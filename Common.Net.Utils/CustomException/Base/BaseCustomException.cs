using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Net.Utils.CustomException.Base
{
    public abstract class BaseCustomException : Exception
    {
        public BaseCustomException()
        {
        }

        public BaseCustomException(string code, string description, Exception innerException)
                            : base(code, innerException)
        {
            Errors.Add(new ErrorContentItem() { Code = code, Description = description });
        }

        protected BaseCustomException(IList<ErrorContentItem> errors)
        {
            Errors.AddRange(errors);
        }

        public BaseCustomException(CustomErrorCode errorCode, params string[] additionalInfo) 
                            : base(errorCode.ToString())
        {
            Errors.Add(new ErrorContentItem(errorCode, additionalInfo));
        }

        public BaseCustomException(CustomErrorCode errorCode, Exception innerException, params string[] additionalInfo)
                            : base(errorCode.ToString(), innerException)
        {
            Errors.Add(new ErrorContentItem(errorCode, additionalInfo));
        }

        public List<ErrorContentItem> Errors { get; } = new List<ErrorContentItem>();

        public object RelatedJson { get; protected set; }

        public bool HasError(CustomErrorCode errorCode)
        {
            return Errors.Any(x => x.Code == errorCode.ToString());
        }
    }

    public class ErrorContentItem
    {
        public ErrorContentItem()
        {
        }

        public ErrorContentItem(CustomErrorCode customErrorCode, params string[] additionalInfo)
        {
            Code = customErrorCode.ToString();

            try
            {
                if (additionalInfo != null && additionalInfo.Any())
                    Description = string.Format(EnumsUtils.GetDescriptionAttribute(customErrorCode), additionalInfo);
                else
                    Description = EnumsUtils.GetDescriptionAttribute(customErrorCode);
            }
            catch
            {
                Description = EnumsUtils.GetDescriptionAttribute(customErrorCode);
            }

            Description = Description.Replace("{0}", string.Empty).Replace("{1}", string.Empty).Replace("{2}", string.Empty).Trim();
        }

        public string Code { get; set;  }

        public string Description { get; set; }

        public override string ToString() => string.Format("BussinesException => Code: {0}. Description: {1}", Code, Description);
    }
}