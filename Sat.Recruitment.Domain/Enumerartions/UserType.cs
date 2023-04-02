using System;
using System.ComponentModel;

namespace Sat.Recruitment.Domain.Enumerartions
{
    public enum UserType
    {
        [Description("Normal")]
        Normal = 1,

        [Description("SuperUser")]
        SuperUser = 2,

        [Description("Premium")]
        Premium = 3
    }
}