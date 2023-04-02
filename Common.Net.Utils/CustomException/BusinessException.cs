using System.Collections.Generic;
using Common.Net.Utils.CustomException.Base;

namespace Common.Net.Utils.CustomException
{
    public class BusinessException : BaseCustomException
    {
        public BusinessException(CustomErrorCode errorCode, params string[] additionalInfo) : base(errorCode, additionalInfo)
        {
        }

        public BusinessException(List<ErrorContentItem> errors) : base(errors)
        {
        }
    }
}