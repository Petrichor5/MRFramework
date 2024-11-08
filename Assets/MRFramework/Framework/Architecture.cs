using System.Collections.Generic;

namespace MRFramework
{
    public interface IArchitecture
    {
	    void OnDispose();
	    
        void RegisterSystem<T>(T system) where T : ISystem;

        void RegisterController<T>(T controller) where T : IController;

        void RegisterUtility<T>(T utility) where T : IUtility;

        T GetSystem<T>() where T : class, ISystem;

        T GetController<T>() where T : class, IController;

        T GetUtility<T>() where T : class, IUtility;

        void SendCommand<T>(T command) where T : ICommand;

        TResult SendCommand<TResult>(ICommand<TResult> command);

        TResult SendQuery<TResult>(IQuery<TResult> query);
    }
    
	public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
	{
		private bool m_Inited = false;

		private HashSet<ISystem> m_Systems = new HashSet<ISystem>();

		private HashSet<IController> m_Controllers = new HashSet<IController>();

		protected static T m_Architecture;

		public static IArchitecture Instance
		{
			get
			{
				if (m_Architecture == null)
				{
					InitArchitecture();
				}

				return m_Architecture;
			}
		}

		private static void InitArchitecture()
		{
			if (m_Architecture == null)
			{
				m_Architecture = new T();
				m_Architecture.Init();

				foreach (var architectureModel in m_Architecture.m_Controllers)
				{
					architectureModel.Init();
				}

				m_Architecture.m_Controllers.Clear();

				foreach (var architectureSystem in m_Architecture.m_Systems)
				{
					architectureSystem.Init();
				}

				m_Architecture.m_Systems.Clear();

				m_Architecture.m_Inited = true;
			}
		}

		public void OnDispose()
		{
			if (m_Architecture != null)
			{
				foreach (var controller in m_Architecture.m_Controllers)
				{
					controller.Dispose();
				}
				m_Architecture.m_Controllers.Clear();

				foreach (var system in m_Architecture.m_Systems)
				{
					system.Dispose();
				}
				m_Architecture.m_Systems.Clear();

				m_Architecture.m_Inited = false;
			}
		}

		protected abstract void Init();

		private IOCContainer mContainer = new IOCContainer();

		public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
		{
			system.SetArchitecture(this);
			mContainer.Register(system);

			if (!m_Inited)
			{
				m_Systems.Add(system);
			}
			else
			{
				system.Init();
			}
		}

		public void RegisterController<TController>(TController controller) where TController : IController
		{
			controller.SetArchitecture(this);
			mContainer.Register(controller);

			if (!m_Inited)
			{
				m_Controllers.Add(controller);
			}
			else
			{
				controller.Init();
			}
		}

		public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility =>
			mContainer.Register(utility);

		public TSystem GetSystem<TSystem>() where TSystem : class, ISystem => mContainer.Get<TSystem>();

		public TController GetController<TController>() where TController : class, IController => mContainer.Get<TController>();

		public TUtility GetUtility<TUtility>() where TUtility : class, IUtility => mContainer.Get<TUtility>();

		public TResult SendCommand<TResult>(ICommand<TResult> command) => ExecuteCommand(command);

		public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand => ExecuteCommand(command);

		public virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
		{
			command.SetArchitecture(this);
			return command.Execute();
		}

		protected virtual void ExecuteCommand(ICommand command)
		{
			command.SetArchitecture(this);
			command.Execute();
		}
		
		public TResult SendQuery<TResult>(IQuery<TResult> query) => DoQuery(query);

		protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
		{
			query.SetArchitecture(this);
			return query.Do();
		}
	}
}