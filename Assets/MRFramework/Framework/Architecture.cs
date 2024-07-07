using System.Collections.Generic;

namespace MRFramework
{
    public interface IArchitecture
    {
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
		private bool mInited = false;

		private HashSet<ISystem> mSystems = new HashSet<ISystem>();

		private HashSet<IController> mControllers = new HashSet<IController>();

		protected static T mArchitecture;

		public static IArchitecture Instance
		{
			get
			{
				if (mArchitecture == null)
				{
					InitArchitecture();
				}

				return mArchitecture;
			}
		}

		private static void InitArchitecture()
		{
			if (mArchitecture == null)
			{
				mArchitecture = new T();
				mArchitecture.Init();

				foreach (var architectureModel in mArchitecture.mControllers)
				{
					architectureModel.Init();
				}

				mArchitecture.mControllers.Clear();

				foreach (var architectureSystem in mArchitecture.mSystems)
				{
					architectureSystem.Init();
				}

				mArchitecture.mSystems.Clear();

				mArchitecture.mInited = true;
			}
		}

		protected abstract void Init();

		private IOCContainer mContainer = new IOCContainer();

		public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
		{
			system.SetArchitecture(this);
			mContainer.Register(system);

			if (!mInited)
			{
				mSystems.Add(system);
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

			if (!mInited)
			{
				mControllers.Add(controller);
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