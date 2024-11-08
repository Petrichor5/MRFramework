namespace MRFramework
{
	#region Architecture

	public interface IBelongToArchitecture
	{
		IArchitecture GetArchitecture();
	}

	public interface ICanSetArchitecture
	{
		void SetArchitecture(IArchitecture architecture);
	}

	#endregion

	#region Model

	public interface ICanGetController : IBelongToArchitecture
	{
	}

	public static class CanGetModelExtension
	{
		public static T GetController<T>(this ICanGetController self) where T : class, IController =>
			self.GetArchitecture().GetController<T>();
	}

	#endregion

	#region System

	public interface ICanGetSystem : IBelongToArchitecture
	{
	}

	public static class CanGetSystemExtension
	{
		public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem =>
			self.GetArchitecture().GetSystem<T>();
	}

	#endregion

	#region Utility

	public interface ICanGetUtility : IBelongToArchitecture
	{
	}

	public static class CanGetUtilityExtension
	{
		public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility =>
			self.GetArchitecture().GetUtility<T>();
	}

	#endregion

	#region Command

	public interface ICanSendCommand : IBelongToArchitecture
	{
	}

	public static class CanSendCommandExtension
	{
		public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new() =>
			self.GetArchitecture().SendCommand<T>(new T());

		public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand =>
			self.GetArchitecture().SendCommand<T>(command);

		public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command) =>
			self.GetArchitecture().SendCommand(command);
	}

	#endregion

	#region Query

	public interface ICanSendQuery : IBelongToArchitecture
	{
	}

	public static class CanSendQueryExtension
	{
		public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query) =>
			self.GetArchitecture().SendQuery(query);
	}

	#endregion
}