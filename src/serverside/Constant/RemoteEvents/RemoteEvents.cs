/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
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

        #region AdminListScript

        public const string OnPlayerSendReport = "OnPlayerSendReport";

        #endregion

        #region AnimationsScript

        public const string OnPlayerAddAnim = "OnPlayerAddAnim";

        #endregion

        #region CoreScript

        public const string ChangePosition = "ChangePosition";

        public const string InvokeWaypointVector = "InvokeWaypointVector";

        #endregion

        #region ScoreBoardScript

        public const string playerlist_pings = "playerlist_pings";

        #endregion

        #region WheelMenuScript

        public const string RequestWheelMenu = "RequestWheelMenu";

        public const string UseWheelMenuItem = "UseWheelMenuItem";

        #endregion

        #region GroupWarehouseScript

        public const string OnPlayerAddWarehouseItem = "OnPlayerAddWarehouseItem";

        public const string OnPlayerPlaceOrder = "OnPlayerPlaceOrder";

        #endregion

        #region CourierWarehouseScript

        public const string OnPlayerTakePackage = "OnPlayerTakePackage";



        #endregion

        #region OffersScript

        public const string OnPlayerCancelOffer = "OnPlayerCancelOffer";

        public const string OnPlayerPayOffer = "OnPlayerPayOffer";

        #endregion

        #region AtmScript

        public const string OnPlayerAtmTake = "OnPlayerAtmTake";

        public const string OnPlayerAtmGive = "OnPlayerAtmGive";

        #endregion

        #region TelephoneBoothScript

        public const string OnPlayerTelephoneBoothCall = "OnPlayerTelephoneBoothCall";

        public const string OnPlayerTelephoneBoothEnd = "OnPlayerTelephoneBoothEnd";

        #endregion

        #region BusStopScript

        public const string RequestBus = "RequestBus";

        #endregion

        #region CarshopScript

        public const string OnPlayerBoughtVehicle = "OnPlayerBoughtVehicle";

        #endregion

        #region CornerBotsScript

        public const string AddCornerBot = "AddCornerBot";

        #endregion

        #region DriveThruScript

        public const string OnPlayerDriveThruBought = "OnPlayerDriveThruBought";

        #endregion

        #region MarketScript

        public const string AddMarketItem = "AddMarketItem";

        public const string OnPlayerBoughtMarketItem = "OnPlayerBoughtMarketItem";

        #endregion
    }
}