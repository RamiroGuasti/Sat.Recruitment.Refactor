using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Net.Utils.AppSettings;
using Common.Net.Utils.CustomException.Base;
using Sat.Recruitment.Business.Interfaces;
using Sat.Recruitment.Business.Services.Base;
using Sat.Recruitment.DataAccess.Interfaces;
using Sat.Recruitment.DataAccess.Persistence.Context;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Enumerartions;

namespace Sat.Recruitment.Business.Services
{
    public class UserService : BaseEntityService<User>, IUserService
    {
        //// NOTE: this constructor if only for test, the inyection resolve isn't implement in Business layer
        //public UserService() : base(new DataContext())
        //{
        //}

        public UserService(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            return GetBaseQueryableCollection().ToList();
        }

        public Task<int> CreateUser(User item)
        {
            ApplyGift(item);

            return Task.Run(() => Add(item)); // return the new Id for the entity (the Add method must be AddAsync...)
        }

        protected override void BusinessValidations(User item)
        {
            BasicPrimitiveValidations(item);

            // another business validations...
        }

        #region Private Methods

        private void BasicPrimitiveValidations(User item)
        {
            AddBusinessValidation(string.IsNullOrWhiteSpace(item.Name), CustomErrorCode.Global_RequiredName);

            AddBusinessValidation(string.IsNullOrWhiteSpace(item.Email), CustomErrorCode.Global_RequiredEmail);

            AddBusinessValidation(string.IsNullOrWhiteSpace(item.Address), CustomErrorCode.GLobal_RequiredAddress);

            AddBusinessValidation(string.IsNullOrWhiteSpace(item.Phone), CustomErrorCode.Global_RequiredPhone);
        }

        private void ApplyGift(User user)
        {
            var percentage = 1m;

            switch (user.UserType)
            {
                case UserType.Normal:
                    percentage = user.Money > AppSettingsProvider.MinMoneyToGif ? AppSettingsProvider.GiftPercentToNormalUserMin : 
                                 user.Money < AppSettingsProvider.MinMoneyToGif && user.Money > AppSettingsProvider.MinMoneyToGifNormalUser 
                                                                                ? AppSettingsProvider.GiftPercentToNormalUserMax : 1;
                    break;

                case UserType.SuperUser:
                    percentage = user.Money > AppSettingsProvider.MinMoneyToGif ? AppSettingsProvider.GiftPercentToSuperUser : 1;
                    break;

                case UserType.Premium:
                    percentage = user.Money > AppSettingsProvider.MinMoneyToGif ? AppSettingsProvider.GiftPercentToPremiumUser : 1;
                    break;
            }

            var gif = user.Money * percentage;

            user.Money += gif;
        }

        #endregion
    }
}