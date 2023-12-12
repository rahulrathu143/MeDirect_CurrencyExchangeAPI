//using System;
//using System.ComponentModel.DataAnnotations;

//namespace Utilities
//{
//	public class CustomerEntityValidateUtility:ValidationAttribute
//	{

//		private long _customerID;
//		private bool _isValidInput;

//        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
//        {
//            var customerID = validationContext.ObjectType.GetProperty("CustomerID").GetValue(validationContext.ObjectInstance) as
//                 string;

//        }

//        public bool CheckCustomerData()
//        {
//            if(CUrrenc)
//        }

//    }
//}

