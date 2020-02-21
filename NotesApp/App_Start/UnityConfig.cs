using NotesApp.Business;
using NotesApp.Controllers;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace NotesApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<INotesRepository, NotesRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
           
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}