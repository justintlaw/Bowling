using Bowling.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bowling.Models
{
    public class Categories
    {
        public static Categories GetCategories(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            Categories cat = session?.GetJson<Categories>("Categories") ?? new Categories();
            cat.Session = session;
            return cat;
        }
        
        [JsonIgnore]
        public ISession Session { get; set; }
        public List<long> SelectedTeamIds { get; set; } = new List<long>();

        public void AddCategory(long id)
        {
            SelectedTeamIds.Add(id);
        }

        public void RemoveCategory(long id)
        {
            SelectedTeamIds.Remove(id);
        }

        public void Clear() => SelectedTeamIds.Clear();
    }
}
