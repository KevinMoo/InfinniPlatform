﻿namespace InfinniPlatform
{
	/// <summary>
	/// Категории тестов.
	/// </summary>
	public static class TestCategories
	{
		/// <summary>
		/// Модульные тесты.
		/// </summary>
		public const string UnitTest = "UnitTest";

		/// <summary>
		/// Стресс-тесты.
		/// </summary>
		public const string StressTest = "StressTest";

		/// <summary>
		/// Тесты на проверку утечек памяти.
		/// </summary>
		public const string MemoryLeakTest = "MemoryLeakTest";

		/// <summary>
		/// Тесты на проверку производительности.
		/// </summary>
		public const string PerformanceTest = "PerformanceTest";

		/// <summary>
		/// Интеграционные тесты.
		/// </summary>
		public const string IntegrationTest = "IntegrationTest";
	}
}