/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Security.Authentication.ExtendedProtection;

namespace VRP.Serverside.Constant.RemoteEvents
{
    public static class RemoteEvents
    {
        public const string PlayerLoginRequested = "playerLoginRequested";
        public const string PlayerLoginPassed = "playerLoginPassed";

        public const string PlayerNotifyRequested = "playerNotifyRequested";

        public const string CharacterSelectRequested = "characterSelectRequested";

        public const string CharacterMoneyChangeRequested = "characterMoneyChangeRequested";

        public const string CharacterShowShardRequested = "characterShowShardRequested";

        public const string PlayerFreeCamRequested = "playerFreeCamRequested";

        public const string PlayerZoneManagerRequested = "playerZoneManagerRequested";

        public const string PlayerMugshotRequested = "playerMugshotRequested";
        public const string PlayerMugshotDestroyRequested = "playerMugshotDestroyRequested";

        public const string PlayerWheelMenuRequested = "playerWheelMenuRequested";

        #region AdminListScript

        public const string OnPlayerSendReport = "onPlayerSendReport";

        #endregion

        #region AnimationsScript

        public const string OnPlayerAddAnim = "onPlayerAddAnim";

        #endregion

        #region CoreScript

        public const string ChangePosition = "changePosition";

        public const string InvokeWaypointVector = "invokeWaypointVector";

        #endregion

        #region ScoreBoardScript

        public const string PlayerListPings = "playerListPings";

        #endregion

        #region WheelMenuScript

        public const string RequestWheelMenu = "requestWheelMenu";

        public const string UseWheelMenuItem = "useWheelMenuItem";

        public const string ShowWheelMenu = "showWheelMenu";

        #endregion

        #region GroupWarehouseScript

        public const string OnPlayerAddWarehouseItem = "onPlayerAddWarehouseItem";

        public const string OnPlayerPlaceOrder = "onPlayerPlaceOrder";

        #endregion

        #region CourierWarehouseScript

        public const string OnPlayerTakePackage = "onPlayerTakePackage";

        #endregion

        #region OffersScript

        public const string OnPlayerCancelOffer = "onPlayerCancelOffer";

        public const string OnPlayerPayOffer = "onPlayerPayOffer";

        #endregion

        #region AtmScript

        public const string OnPlayerAtmTake = "onPlayerAtmTake";

        public const string OnPlayerAtmGive = "onPlayerAtmGive";

        #endregion

        #region TelephoneBoothScript

        public const string OnPlayerTelephoneBoothCall = "onPlayerTelephoneBoothCall";

        public const string OnPlayerTelephoneBoothEnd = "onPlayerTelephoneBoothEnd";

        #endregion

        #region BusStopScript

        public const string RequestBus = "requestBus";

        #endregion

        #region CarshopScript

        public const string OnPlayerBoughtVehicle = "onPlayerBoughtVehicle";

        #endregion

        #region CornerBotsScript

        public const string AddCornerBot = "addCornerBot";

        #endregion

        #region DriveThruScript

        public const string OnPlayerDriveThruBought = "onPlayerDriveThruBought";

        #endregion

        #region MarketScript

        public const string AddMarketItem = "addMarketItem";

        public const string OnPlayerBoughtMarketItem = "onPlayerBoughtMarketItem";

        #endregion

        #region ItemScript

        public const string SelectedItem = "selectedItem";

        public const string UseItem = "useItem";

        public const string InformationsItem = "informationsItem";

        public const string UsingInformationsItem = "usingInformationsItem";

        public const string BackToItemList = "backToItemList";

        public const string OnPlayerTelephoneCall = "onPlayerTelephoneCall";

        public const string OnPlayerTelephoneTurnoff = "onPlayerTelephoneTurnoff";

        public const string OnPlayerPullCellphoneRequest = "onPlayerPullCellphoneRequest";

        public const string OnPlayerCellphonePickUp = "onPlayerCellphonePickUp";

        public const string OnPlayerCellphoneEnd = "onPlayerCellphoneEnd";

        public const string OnPlayerTelephoneContactAdded = "onPlayerTelephoneContactAdded";

        #endregion

        #region VehicleIndicatorScript

        public const string ToggleIndicatorLeft = "toggleIndicatorLeft";

        public const string ToggleIndicatorRight = "toggleIndicatorRight";

        #endregion

        #region VehicleScript

        public const string OnPlayerSelectedVehicle = "onPlayerSelectedVehicle";

        public const string OnPlayerSpawnVehicle = "onPlayerSpawnVehicle";

        public const string OnPlayerParkVehicle = "onPlayerParkVehicle";

        public const string OnPlayerInformationsVehicle = "onPlayerInformationsVehicle";

        public const string OnPlayerInformationsInVehicle = "onPlayerInformationsInVehicle";

        public const string OnPlayerChangeLockVehicle = "onPlayerChangeLockVehicle";

        public const string OnPlayerEngineStateChangeVehicle = "onPlayerEngineStateChangeVehicle";

        public const string OnPlayerEnterVehicle = "onPlayerEnterVehicle";

        #endregion
    }
}