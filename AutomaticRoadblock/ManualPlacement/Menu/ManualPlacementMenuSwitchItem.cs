using System;
using System.Diagnostics.CodeAnalysis;
using AutomaticRoadblocks.AbstractionLayer;
using AutomaticRoadblocks.Localization;
using AutomaticRoadblocks.Menu;
using AutomaticRoadblocks.Menu.Switcher;
using RAGENativeUI;

namespace AutomaticRoadblocks.ManualPlacement.Menu
{
    public class ManualPlacementMenuSwitchItem : IMenuSwitchItem, IDisposable
    {
        private readonly IGame _game;
        private readonly IManualPlacement _manualPlacement;

        private bool _running = true;

        public ManualPlacementMenuSwitchItem(IGame game, IManualPlacement manualPlacement)
        {
            _game = game;
            _manualPlacement = manualPlacement;
        }

        /// <inheritdoc />
        public UIMenu Menu { get; } = new(IoC.Instance.GetInstance<ILocalizer>()[LocalizationKey.MenuTitle],
            "~b~" + IoC.Instance.GetInstance<ILocalizer>()[LocalizationKey.MenuSubtitle]);

        /// <inheritdoc />
        public MenuType Type => MenuType.ManualPlacement;

        /// <inheritdoc />
        public string DisplayText => AutomaticRoadblocksPlugin.MenuManualPlacement;

        /// <inheritdoc />
        public void Dispose()
        {
            _running = false;
        }

        [IoC.PostConstruct]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Init()
        {
            Process();
        }

        private void Process()
        {
            _game.NewSafeFiber(() =>
            {
                while (_running)
                {
                    _game.FiberYield();

                    if (Menu.Visible)
                    {
                        _manualPlacement.CreatePreview();
                    }
                    else
                    {
                        _manualPlacement.DeletePreview();
                    }
                }
            }, "ManualPlacementMenuSwitchItem.Process");
        }
    }
}