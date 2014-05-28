using SalaryEntities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facsal.Validators
{
    public class BreezeValidator : IBreezeValidator
    {
        public BreezeValidator() {}

        public bool BeforeSaveEntity(Breeze.ContextProvider.EntityInfo entityInfo)
        {
            return true;
        }

        public Dictionary<Type, List<Breeze.ContextProvider.EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<Breeze.ContextProvider.EntityInfo>> saveMap)
        {
            return saveMap;
        }
    }
}