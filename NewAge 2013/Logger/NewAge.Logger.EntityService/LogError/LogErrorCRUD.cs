using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Logger.Data.Entities;

namespace NewAge.Logger.EntityService
{
    public partial class LogErrorService : AbstractEntityService<LogError, NewAgeLoggerContainer>
    {
        public LogErrorService()
        {
            this._repository = new NewAgeLoggerContainer();
        }

        public override IQueryable<LogError> Where(Dictionary<string, object> criteria = null, bool lazyEnable = false)
        {
            var t = (from c in this._repository.LogErrors select c);
            return this.Where(t, criteria, lazyEnable);
        }

        public override bool Exist(LogError entity)
        {
            return this.GetObjectSet().Where(c => c.Id == entity.Id).Count() > 0;
        }

        public override System.Data.Objects.ObjectSet<LogError> GetObjectSet()
        {
            return this._repository.LogErrors;
        }
    }
}
