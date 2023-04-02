using System.ComponentModel;

namespace Common.Net.Utils.CustomException.Base
{
    public enum CustomErrorCode
    {
        #region Global

        [Description("{0}")]
        General,

        [Description("Invalid item")]
        Global_InvalidItem,

        [Description("Invalid update. The id item is required")]
        Global_UpdateItemIdValidation,

        [Description("No Autorizado")]
        Global_Unauthorized,

        [Description("The name is required")]
        Global_RequiredName,

        [Description("The email is required")]
        Global_RequiredEmail,

        [Description("The address is required")]
        GLobal_RequiredAddress,

        [Description("The phone is required")]
        Global_RequiredPhone,

        #endregion

    }
}