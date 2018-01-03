using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;

namespace NewAge.Logger.EntityService
{
    public abstract class AbstractEntityService<T, O>
        where T : class
        where O : ObjectContext
    {

        public O _repository
        {
            get;
            set;
        }


        public AbstractEntityService()
        {

        }

        public T Create(T entity)
        {
            this.GetObjectSet().AddObject(entity);
            return this._repository.SaveChanges() != 0 ? entity : default(T);
        }

        public bool Update(T entity)
        {
            return this.Exist(entity) ? this.Create(entity).Equals(default(T)) : false;
        }

        public T Select(Dictionary<string, object> criteria, bool lazyEnable = false)
        {
            return this.Where(criteria, lazyEnable).FirstOrDefault();
        }

        public bool Delete(T entity)
        {
            if (Exist(entity))
            {
                this.GetObjectSet().DeleteObject(entity);
                return this._repository.SaveChanges() != 0;
            }
            return false;
        }


        public abstract IQueryable<T> Where(Dictionary<string, object> criteria = null, bool lazyEnable = false);

        protected IQueryable<T> Where(IQueryable<T> source, Dictionary<string, object> criteria = null, bool lazyEnable = false)
        {
            StringBuilder query = new StringBuilder();
            List<object> param = new List<object>();
            if (criteria != null)
            {
                foreach (var key in criteria.Keys)
                {

                    var t = typeof(T);
                    var propertyInfo = t.GetProperty(key);
                    if (propertyInfo != null)
                    {
                        var refObj = criteria[key];
                        if (this.LikeFields().Contains(key) && refObj is string)
                        {
                            query.Append(key + ".Contains(@0) and");
                        }
                        else
                        {
                            query.Append(key + ".Equals(@0) and");


                        }
                        param.Add((object)refObj);
                    }
                }
            }
            query.Append(" true");
            var res = source.Where(query.ToString(), param.ToArray());
            return res;
        }

        protected virtual HashSet<string> LikeFields()
        {
            return new HashSet<string>() { };
        }

        public int Count(Dictionary<string, object> criteria = null)
        {
            return this.Where(criteria).Count();
        }

        public abstract bool Exist(T entity);

        public abstract ObjectSet<T> GetObjectSet();

        public virtual IQueryable<T> Paginate(int pageNumber, int pageSize = 50, Dictionary<string, object> criteria = null, bool lazyEnable = false)
        {
            pageNumber = pageNumber - 1 > 0 ? pageNumber : 0;
            return this.Where(criteria, lazyEnable).Skip(pageNumber * pageSize).Take(pageSize);
        }

    }
}