using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using SpacewarsBlazor.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Graphics
{
    public class SpacewarsCanvasContext
    {
        private Player player;
        private Canvas2DContext context;

        public SpacewarsCanvasContext(Player player, Canvas2DContext context)
        {
            this.player = player;
            this.context = context;
        }

        public static async Task<SpacewarsCanvasContext> CreateAsync(Player player, BECanvasComponent _canvasReference)
        {
            var context = await _canvasReference.CreateCanvas2DAsync();

            return new SpacewarsCanvasContext(player, context);
        }

        public async Task RenderFrameAsync()
        {
            await this.context.BeginBatchAsync();

            await DrawBackGround();

            await DrawShips();

            await DrawBullets();

            await DrawOwnShipIndicator();

            await DrawShipEnergyIndicator();

            await this.context.EndBatchAsync();
        }

        private async Task DrawBackGround()
        {
            if (player.Inactive) await this.context.SetFillStyleAsync("#ff0000");
            else if (player.CurrentlyDead) await this.context.SetFillStyleAsync("#400000");
            else await this.context.SetFillStyleAsync("#000040");

            await this.context.FillRectAsync(0, 0, Game.MaxX, Game.MaxY);
        }

        private async Task DrawShips()
        {
            var allPlayers = Game.PlayerSnapshot;
            foreach (var _player in allPlayers)
            {
                await DrawShip(_player);
            }
        }

        private async Task DrawShip(Player _player)
        {
            var clr = _player.Color;
            if (_player.CurrentlyDead) clr = "#ff0000";

            await this.context.SaveAsync();

            await this.context.TranslateAsync(_player.X, _player.Y);
            await this.context.RotateAsync(_player.Heading);
            await this.context.TranslateAsync(-_player.X, -_player.Y);

            await this.context.BeginPathAsync();
            await this.context.ArcAsync(_player.X, _player.Y - (_player.Size / 2), _player.Size, 0, Math.PI, false);
            await this.context.SetFillStyleAsync(clr);
            await this.context.FillAsync();

            await this.context.BeginPathAsync();
            await this.context.ArcAsync(_player.X, _player.Y, _player.Size, 0, 0.5 * Math.PI, false);
            await this.context.SetFillStyleAsync(clr);
            await this.context.FillAsync();

            await this.context.BeginPathAsync();
            await this.context.ArcAsync(_player.X, _player.Y, _player.Size, 0.5 * Math.PI, Math.PI, false);
            await this.context.SetFillStyleAsync(clr);
            await this.context.FillAsync();

            await this.context.RestoreAsync();
        }

        private async Task DrawBullets()
        {
            var allBullets = Game.BulletSnapshot;
            foreach (var _bullet in allBullets)
            {
                await DrawBullet(_bullet);
            }
        }

        private async Task DrawBullet(Bullet _bullet)
        {
            await this.context.BeginPathAsync();
            await this.context.ArcAsync(_bullet.X, _bullet.Y, _bullet.Size, 0, 2 * Math.PI, false);
            await this.context.SetFillStyleAsync(_bullet.Color);
            await this.context.FillAsync();
        }

        private async Task DrawOwnShipIndicator()
        {
            await this.context.BeginPathAsync();
            await this.context.ArcAsync(player.X, player.Y, 20, 0, 2 * Math.PI, false);
            await this.context.SetStrokeStyleAsync("#ffffff");
            await this.context.StrokeAsync();
        }

        private async Task DrawShipEnergyIndicator()
        {
            await this.context.SetFontAsync("24px Roboto");
            await this.context.SetFillStyleAsync("#ffffff");
            await this.context.FillTextAsync($"Energy: {player.Energy}", 550, 800);
        }
    }
}
