using GameLib;
using GameLib.Messages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SimpleGameApp
{
    public class GameViewController
    {
        public enum GameAttackResult { Normal, Invalid, GameOver };

        public string ActionMessage { get; set; }
        public Board GameBoard { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }
        public BattleResult CurrentAttack { get; set; }
        public DateTime? StatusTime { get; set; }
        Dictionary<SKRect, Zone> HitMap { get; set; }

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

            // Is it game over?
            if (!GameBoard.StillPlaying())
            {
                ActionMessage = "Winner: " + GameBoard.Winner.Color.ToString();
                var pg = new GameOverPage(GameBoard);
                return GameAttackResult.GameOver;
            }

            // Display result of attack
            ActionMessage = $"Battle Result: {result.Plan.Attacker.X + 1}/{result.Plan.Attacker.Y + 1} vs {result.Plan.Defender.X + 1}/{result.Plan.Defender.Y + 1}:";
            if (result.AttackSucceeded)
            {
                ActionMessage += " Victory!";
            }
            else
            {
                ActionMessage += " Failed.";
            }
            StatusTime = DateTime.UtcNow;
            CurrentAttack = result;
            return GameAttackResult.Normal;
        }

        public void Paint(object sender, SKPaintSurfaceEventArgs e)
        {
            // Clear canvas first
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            e.Surface.Canvas.GetDeviceClipBounds(out var bounds);

            // Start building a hit map
            Dictionary<SKRect, Zone> dict = new Dictionary<SKRect, Zone>();

            // Line height: We need 5 lines for status, plus game board height
            float cellHeight = (bounds.Height / (GameBoard.Height + 5));

            // Scale font to size of screen
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.Black,
                TextSize = (float)(cellHeight * 0.8)
            };

            // Reserve room for status information
            var status_height = cellHeight * 5;
            var board_height = bounds.Height - status_height;
            var board_width = bounds.Width;

            // Draw strengths of each player
            var player_width = (bounds.Width / (GameBoard.Players.Count));
            var padding = cellHeight * 0.1f;
            SKRect player_box = new SKRect();
            player_box.Top = padding;
            player_box.Bottom = cellHeight - padding;
            for (int i = 0; i < GameBoard.Players.Count; i++)
            {
                player_box.Left = (player_width * i) + padding;
                player_box.Right = player_box.Left + (player_box.Bottom - player_box.Top);
                SKPaint p = new SKPaint()
                {
                    Color = ColorFrom(GameBoard.Players[i].Color),
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

            // Draw status
            DrawCenteredText(canvas, $"Round {GameBoard.Round}: {GameBoard.Players[GameBoard.CurrentTurn].Name}", 0, cellHeight * 2, bounds.Width, cellHeight, textPaint);
            if (!String.IsNullOrEmpty(ActionMessage))
            {
                DrawCenteredText(canvas, ActionMessage, 0, cellHeight * 3, bounds.Width, cellHeight, textPaint);
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
                    color = ColorFrom(z.Owner.Color);
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
                DrawCenteredText(canvas, z.Strength.ToString(), r, textPaint);

                // Keep track of screen view map
                dict[r] = z;
            }

            // Update viewmap
            HitMap = dict;
        }

        public GameAttackResult HandleTouch(object sender, SKTouchEventArgs e)
        {
            if (!GameBoard.CurrentPlayer.IsHuman) return GameAttackResult.Invalid;

            // Figure out what zone was touched
            var zone = HitTestZone(e.Location);
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
