using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace MVCAssignment.Models
{
    public class NHibernateSessions
    {
        public static ISession OpenSession()
        {
            /*var cfg = new Configuration();

            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = "Data Source=MUTTAKEELAP\\THINKPAD;Initial Catalog=studentdb;Integrated Security=True";
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2012Dialect>();
            });*/
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath
            (@"~\NHibernate\nhibernate.cfg.xml");
            configuration.Configure(configurationPath);

            var studentConfigurationFile = HttpContext.Current.Server.MapPath
                (@"~\NHibernate\Student.hbm.xml");
            configuration.AddFile(studentConfigurationFile);

            /* cfg.AddAssembly(Assembly.GetExecutingAssembly()); */

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}