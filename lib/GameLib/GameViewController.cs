using GameLib;
using GameLib.Messages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameLib
{
    public class GameViewController
    {
        public enum GameAttackResult { Normal, Invalid, GameOver };

        public Board GameBoard { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }
        public BattleResult CurrentAttack { get; set; }
        public DateTime? StatusTime { get; set; }
        Dictionary<SKRect, Zone> HitMap { get; set; }
        public SKRect? EndTurnRect { get; set; }

        public GameAttackResult ExecuteAttackPlan(AttackPlan plan)
        {
            var result = GameBoard.BattleRule.Attack(GameBoard, GameBoard.Players[GameBoard.CurrentTurn], plan);
            this.Attacking = null;

            // Safety check
            if (result.AttackWasInvalid)
            {
                return GameAttackResult.Invalid;
            }

            // Update the board
            result.UpdateBoardTask.RunSynchronously();

            // Display result of attack
            StatusTime = DateTime.UtcNow;
            CurrentAttack = result;

            // Is the game over?
            if (!GameBoard.StillPlaying())
            {
                return GameAttackResult.GameOver;
            }
            return GameAttackResult.Normal;
        }

        public void Paint(object sender, SKCanvas canvas)
        {
            // Clear canvas first
            canvas.Clear(SKColors.White);
            canvas.GetDeviceClipBounds(out var bounds);

            // Start building a hit map
            Dictionary<SKRect, Zone> dict = new Dictionary<SKRect, Zone>();

            // Line height: We need 5 lines for status, plus game board height
            float cellHeight = (bounds.Height / (GameBoard.Height + 3));

            // Scale font to size of screen
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.Black,
                TextSize = (float)(cellHeight * 0.8)
            };

            // Reserve room for status information
            var status_height = cellHeight * 1.5f;
            var board_height = bounds.Height - status_height;
            var board_width = bounds.Width;

            // Draw strengths of each player
            var player_width = (bounds.Width / (GameBoard.Players.Count));
            var padding = cellHeight * 0.1f;
            SKRect player_box = new SKRect();
            player_box.Top = padding + padding;
            player_box.Bottom = cellHeight - padding - padding;
            for (int i = 0; i < GameBoard.Players.Count; i++)
            {
                var xpos = player_width * i;

                // Highlight active player
                if (GameBoard.CurrentTurn == i) {
                    canvas.DrawRect(new SKRect(xpos, 0, xpos + player_width, cellHeight), new SKPaint() { Color = SKColors.Gray, Style = SKPaintStyle.Fill });
                }

                // Adjust dimensions of box and draw their color indicator
                player_box.Left = xpos + padding + padding;
                player_box.Right = player_box.Left + (player_box.Bottom - player_box.Top) - padding;
                SKPaint p = new SKPaint()
                {
                    Color = GameBoard.Players[i].Color,
                    Style = SKPaintStyle.Fill,
                };
                canvas.DrawRect(player_box, p);
                SKPaint border = new SKPaint()
                {
                    Color = Lighten(p.Color, 0.20f),
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = padding,
                };
                canvas.DrawRect(player_box, border);
                canvas.DrawText(GameBoard.Players[i].CurrentStrength.ToString(), (player_box.Right + (padding * 2)), player_box.Bottom, textPaint);
            }

            // Figure out cell height and width
            float cellWidth = board_width * 1.0f / GameBoard.Width;

            // Draw each zone
            foreach (var z in GameBoard.Zones)
            {
                bool isAttacking = (z == Attacking || z == Defending);

                // Figure out color to use
                SKColor color = new SKColor(50, 50, 50);
                if (z.Owner == null)
                {
                    color = new SKColor(50, 50, 50);
                }
                else
                {
                    color = z.Owner.Color;
                    if (isAttacking)
                    {
                        color = Lighten(color, 0.7f);
                    }
                }

                // Figure out rectangle
                SKRect r = new SKRect();
                r.Left = cellWidth * z.X;
                r.Top = status_height + cellHeight * z.Y;
                r.Right = r.Left + cellWidth;
                r.Bottom = r.Top + cellHeight;

                // Draw rectangle
                SKPaint p = new SKPaint()
                {
                    Color = color,
                    Style = SKPaintStyle.Fill,
                };
                canvas.DrawRect(r, p);

                // Draw border if attacking
                if (isAttacking)
                {
                    SKPaint border = new SKPaint()
                    {
                        Color = SKColors.Black,
                        Style = SKPaintStyle.Stroke,
                    };
                    canvas.DrawRect(r, border);
                }

                // Draw strength centered in the box
                //DrawCenteredText(canvas, z.Strength.ToString(), r, textPaint);
                DrawPips(canvas, r, color, z.Strength);

                // Keep track of screen view map
                dict[r] = z;
            }

            // Do we need to draw the "End Turn" button?
            EndTurnRect = null;
            if (GameBoard.CurrentPlayer.IsHuman)
            {
                var p4 = padding;
                EndTurnRect = new SKRect(0 + p4, bounds.Bottom - (cellHeight * 1.5f) + p4, bounds.Right - p4, bounds.Bottom - p4);
                SKRoundRect rr = new SKRoundRect(EndTurnRect.Value, padding, padding);
                canvas.DrawRoundRect(rr, new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColors.DarkGray });
                canvas.DrawRoundRect(rr, new SKPaint() { Style = SKPaintStyle.Stroke, Color = SKColors.LightGray });
                DrawCenteredText(canvas, "END TURN", EndTurnRect.Value, textPaint);
            }

            // Update viewmap
            HitMap = dict;
        }

        private void DrawPips(SKCanvas canvas, SKRect r, SKColor color, int strength)
        {
            var pip_size = Math.Max(r.Height * 0.05f, r.Width * 0.05f);
            var vertical_unit = r.Height * 0.375f;
            var horizontal_unit = r.Width * 0.375f;
            var vertical_half = r.Height / 2;
            var horizontal_half = r.Width / 2;
            var x_coords = new float[] { r.Left + horizontal_unit, r.Left + horizontal_half, r.Right - horizontal_unit };
            var y_coords = new float[] { r.Top + vertical_unit, r.Top + vertical_half, r.Bottom - vertical_unit };
            for (int i = 0; i < strength; i++)
            {
                var x = (int)(i % 3);
                var y = (int)(i / 3);
                canvas.DrawCircle(new SKPoint() { X = x_coords[i % 3], Y = y_coords[i / 3] }, pip_size, new SKPaint() { Color = Lighten(color, 0.60f), Style = SKPaintStyle.Fill, IsAntialias = true });
            }
        }

        public GameAttackResult TakeBotAction()
        {
            // Can't take bot actions for a human
            if (GameBoard.CurrentPlayer.IsHuman) return GameAttackResult.Invalid;

            // Okay, we're a bot, let's pick an attack
            var plan = GameBoard.CurrentPlayer.Bot.PickNextAttack(GameBoard, GameBoard.CurrentPlayer);

            // No attack means our turn is over
            if (plan != null)
            {
                return ExecuteAttackPlan(plan);
            }

            // No attack means turn is over
            GameBoard.EndTurn();
            return GameAttackResult.Normal;
        }

        public GameAttackResult HandleTouch(object sender, SKPoint location)
        {
            if (!GameBoard.CurrentPlayer.IsHuman) return GameAttackResult.Invalid;

            // Was this an end turn touch?
            if (EndTurnRect.HasValue)
            {
                if (EndTurnRect.Value.Contains(location))
                {
                    GameBoard.EndTurn();
                    return GameAttackResult.Normal;
                }
            }

            // Figure out what zone was touched
            var zone = HitTestZone(location);
            if (zone == null) return GameAttackResult.Invalid;

            // Set an attacker
            if (this.Attacking == null)
            {
                if (zone.Owner.Number != GameBoard.CurrentTurn)
                {
                    return GameAttackResult.Invalid;
                }
                this.Attacking = zone;
                return GameAttackResult.Normal;
            }

            // Setup attack plan
            var plan = new AttackPlan()
            {
                Attacker = Attacking,
                Defender = zone
            };

            // Execute the attack and see what the result is
            ExecuteAttackPlan(plan);
            if (!GameBoard.StillPlaying()) return GameAttackResult.GameOver;
            return GameAttackResult.Normal;
        }

        private Zone HitTestZone(SKPoint location)
        {
            foreach (var kvp in HitMap)
            {
                if (kvp.Key.Contains(location))
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        private SKColor ColorFrom(Color color)
        {
            return new SKColor((byte)color.R, (byte)color.G, (byte)color.B);
        }


        private void DrawCenteredText(SKCanvas canvas, string text, float x, float y, float w, float h, SKPaint paint)
        {
            DrawCenteredText(canvas, text, new SKRect(x, y, x + w, y + h), paint);
        }

        private void DrawCenteredText(SKCanvas canvas, string text, SKRect rect, SKPaint paint)
        {
            SKRect bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            // Horizontal padding is normal
            var padding_x = (rect.Width - bounds.Width) / 2;

            // Vertical padding is done differently: the Y position is the bottom baseline of the text
            var padding_y = (rect.Height - bounds.Height) / 2;

            // Position text within box
            canvas.DrawText(text, rect.Left + padding_x, rect.Top + rect.Height - padding_y, paint);
        }

        public static SKColor Lighten(SKColor color, float pct)
        {
            float red = (float)color.Red;
            float green = (float)color.Green;
            float blue = (float)color.Blue;

            red = (255 - red) * pct + red;
            green = (255 - green) * pct + green;
            blue = (255 - blue) * pct + blue;

            return new SKColor((byte)red, (byte)green, (byte)blue);
        }
    }
}
