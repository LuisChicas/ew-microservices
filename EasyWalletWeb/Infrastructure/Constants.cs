using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Infrastructure
{
    public class Constants
    {
        public const int ExpensesCategoryTypeID = 1;
        public const int IncomesCategoryTypeID = 2;

        public const string OthersCategoryNameEN = "Others";
        public const string OthersCategoryNameES = "Otros";

        public const string InstructionsNamePrefix = "ins_";
        public const string InstructionNameWelcome = "welcome";
        public const string InstructionNameNewCategory = "newcategory";
        public const string InstructionNameCategories = "categories";
        public const string InstructionNameBalance = "balance";
        public const string InstructionNameMonthly = "monthly";
        public const string InstructionNameHistory = "history";
    }
}
