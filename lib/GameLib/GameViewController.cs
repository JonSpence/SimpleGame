using GameLib;
using GameLib.Animations;
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
        //public enum GameAttackResult { Normal, Invalid, GameOver };

        public Board GameBoard { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }
        //public ActionResult CurrentAttack { get; set; }
        public BaseAnimation Playing { get; set; }
        public Queue<BaseAnimation> PlayQueue { get; set; }

        public GameViewController()
        {
            PlayQueue = new Queue<BaseAnimation>();
            Playing = null;
        }

        public void ExecuteAttackPlan(AttackPlan plan)
        {
            var result = GameBoard.BattleRule.Attack(GameBoard, GameBoard.Players[GameBoard.CurrentTurn], plan);

            // Safety check
            if (result.AttackWasInvalid) return;

            // Set up all animations in sequence
            if (result.Animations != null)
            {
                foreach (var a in result.Animations)
                {
                    PlayQueue.Enqueue(a);
                }
            }

            // Clear attacker animation if any
            this.Attacking = null;
        }

        public RenderDimensions Dimensions { get; set; }

        /// <summary>
        /// Recalculate dimensions when the size of the display changes
        /// </summary>
        /// <returns>The dimensions.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="board"></param>
        private void Resize(int width, int height, Board board)
        {
            if (Dimensions == null || !Dimensions.Match(width, height, board))
            {
                Dimensions = new RenderDimensions(width, height, board);
            }
        }

        public void Paint(object sender, SKCanvas canvas)
        {
            // See if someone went sneaky and changed our dimensions
            canvas.GetDeviceClipBounds(out var bounds);
            Resize(bounds.Width, bounds.Height, GameBoard);

            // Update currently playing animation
            bool end_of_anim = false;
            if (Playing != null)
            {
                end_of_anim = Playing.Heartbeat();
            }

            // Clear canvas first
            canvas.Clear(SKColors.White);

            // Scale font to size of screen
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.Black,
                TextSize = Dimensions.CellHeight
            };

            // Draw strengths of each player
            DrawPlayerScores(canvas, textPaint);

            // Draw each zone
            foreach (var z in GameBoard.Zones)
            {
                bool isAttacking = (z == Attacking || z == Defending);

                // Figure out color to use
                SKColor color = GetBaseColor(z);
                if (Playing != null)
                {
                    color = Playing.RecolorZone(z);
                }
                if (isAttacking)
                {
                    color = Lighten(color, 0.7f);
                }

                // Figure out rectangle
                SKRect r = Dimensions.ZoneToRect[z];

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
                DrawPips(canvas, r, color, z.Strength);
            }

            // Do we need to draw the "End Turn" button?
            if (GameBoard.CurrentPlayer.IsHuman)
            {
                canvas.DrawRoundRect(Dimensions.EndTurnRect, new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColors.DarkGray });
                canvas.DrawRoundRect(Dimensions.EndTurnRect, new SKPaint() { Style = SKPaintStyle.Stroke, Color = SKColors.LightGray });
                DrawCenteredText(canvas, "END TURN", Dimensions.EndTurnRect.Rect, textPaint);
            }

            // Are we ready for the next animation?
            if (end_of_anim || Playing == null)
            {
                if (PlayQueue.Count > 0)
                {
                    Playing = PlayQueue.Dequeue();
                }
                else
                {
                    // If the current player is a bot, do another attack
                    if (!GameBoard.CurrentPlayer.IsHuman)
                    {
                        TakeBotAction();
                    }
                    else
                    {
                        Playing = null;
                    }
                }
            }
        }

        /// <summary>
        /// Basic color for the zone without any other items affecting it
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static SKColor GetBaseColor(Zone z)
        {
            if (z.Owner != null)
            {
                return z.Owner.Color;
            }
            else
            {
                return new SKColor(40, 40, 40);
            }
        }

        private void DrawPlayerScores(SKCanvas canvas, SKPaint textPaint)
        {
            SKRect player_box = new SKRect();
            player_box.Top = Dimensions.PlayerBoxPadding;
            player_box.Bottom = Dimensions.PlayerBoxBottom;
            for (int i = 0; i < GameBoard.Players.Count; i++)
            {
                var xpos = Dimensions.PlayerWidth * i;

                // Highlight active player
                if (GameBoard.CurrentTurn == i)
                {
                    canvas.DrawRect(new SKRect(xpos, 0, xpos + Dimensions.PlayerWidth, Dimensions.CellHeight), new SKPaint() { Color = SKColors.Gray, Style = SKPaintStyle.Fill });
                }

                // Adjust dimensions of box and draw their color indicator
                player_box.Left = xpos + Dimensions.PlayerBoxPadding;
                player_box.Right = player_box.Left + (player_box.Bottom - player_box.Top) - Dimensions.PlayerStatusPadding;
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
                    StrokeWidth = Dimensions.PlayerStatusPadding,
                };
                canvas.DrawRect(player_box, border);
                canvas.DrawText(GameBoard.Players[i].CurrentStrength.ToString(), (player_box.Right + (Dimensions.PlayerBoxPadding)), player_box.Bottom, textPaint);
            }
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

        public void TakeBotAction()
        {
            // Can't take bot actions for a human
            if (GameBoard.CurrentPlayer.IsHuman)
            {
                return;
            }

            // Okay, we're a bot, let's pick an attack
            var plan = GameBoard.CurrentPlayer.Bot.PickNextAttack(GameBoard, GameBoard.CurrentPlayer);

            // No attack means our turn is over
            if (plan != null)
            {
                ExecuteAttackPlan(plan);
            }
            else
            {
                Playing = GameBoard.EndTurn();
            }
        }

        public void HandleTouch(object sender, SKPoint location)
        {
            if (!GameBoard.CurrentPlayer.IsHuman)
            {
                return;
            }

            // Was this an end turn touch?
            if (Dimensions.EndTurnRect.Rect.Contains(location))
            {
                Playing = GameBoard.EndTurn();
            }

            // Figure out what zone was touched
            var zone = HitTestZone(location);
            if (zone == null)
            {
                return;
            }

            // Set an attacker
            if (this.Attacking == null)
            {
                if (zone.Owner.Number != GameBoard.CurrentTurn)
                {
                    return;
                }
                this.Attacking = zone;
                return;
            }

            // Setup attack plan
            var plan = new AttackPlan()
            {
                Attacker = Attacking,
                Defender = zone
            };

            // Execute the attack and see what the result is
            ExecuteAttackPlan(plan);
        }

        private Zone HitTestZone(SKPoint location)
        {
            foreach (var kvp in Dimensions.ZoneToRect)
            {
                if (kvp.Value.Contains(location))
                {
                    return kvp.Key;
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
