﻿using Blazor.Extensions;
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
        private Canvas2DContext _context;

        public SpacewarsCanvasContext(Player player, Canvas2DContext context)
        {
            this.player = player;
            this._context = context;
        }

        public static async Task<SpacewarsCanvasContext> CreateAsync(Player player, BECanvasComponent _canvasReference)
        {
            var context = await _canvasReference.CreateCanvas2DAsync();

            return new SpacewarsCanvasContext(player, context);
        }

        public async Task RenderFrameAsync()
        {
            await this._context.BeginBatchAsync();

            if (player.Inactive) await this._context.SetFillStyleAsync("#ff0000");
            else if (player.CurrentlyDead) await this._context.SetFillStyleAsync("#400000");
            else await this._context.SetFillStyleAsync("#000040");

            await this._context.FillRectAsync(0, 0, Game.MaxX, Game.MaxY);

            await this._context.BeginPathAsync();
            await this._context.ArcAsync(player.X, player.Y, 20, 0, 2 * Math.PI, false);
            await this._context.SetStrokeStyleAsync("#ffffff");
            await this._context.StrokeAsync();

            await this._context.SetFontAsync("24px Roboto");
            await this._context.SetFillStyleAsync("#ffffff");
            await this._context.FillTextAsync($"Energy: {player.Energy}", 550, 800);

            var allPlayers = Game.PlayerSnapshot;
            foreach (var _player in allPlayers)
            {
                await DrawShip(_player);
            }

            var allBullets = Game.BulletSnapshot;
            foreach (var _bullet in allBullets)
            {
                await DrawBullet(_bullet);
            }

            await this._context.EndBatchAsync();
        }

        private async Task DrawShip(Player _player)
        {
            var clr = _player.Color;
            if (_player.CurrentlyDead) clr = "#ff0000";

            await this._context.SaveAsync();

            await this._context.TranslateAsync(_player.X, _player.Y);
            await this._context.RotateAsync(_player.Heading);
            await this._context.TranslateAsync(-_player.X, -_player.Y);

            await this._context.BeginPathAsync();
            await this._context.ArcAsync(_player.X, _player.Y - (_player.Size / 2), _player.Size, 0, Math.PI, false);
            await this._context.SetFillStyleAsync(clr);
            await this._context.FillAsync();

            await this._context.BeginPathAsync();
            await this._context.ArcAsync(_player.X, _player.Y, _player.Size, 0, 0.5 * Math.PI, false);
            await this._context.SetFillStyleAsync(clr);
            await this._context.FillAsync();

            await this._context.BeginPathAsync();
            await this._context.ArcAsync(_player.X, _player.Y, _player.Size, 0.5 * Math.PI, Math.PI, false);
            await this._context.SetFillStyleAsync(clr);
            await this._context.FillAsync();

            await this._context.RestoreAsync();
        }

        private async Task DrawBullet(Bullet _bullet)
        {
            await this._context.BeginPathAsync();
            await this._context.ArcAsync(_bullet.X, _bullet.Y, _bullet.Size, 0, 2 * Math.PI, false);
            await this._context.SetFillStyleAsync(_bullet.Color);
            await this._context.FillAsync();
        }
    }
}