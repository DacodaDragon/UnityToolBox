using NUnit.Framework;
using System;
using System.Diagnostics;

namespace ToolBox.UnitTests
{
	public class DummyMessage : Message
	{
		public readonly string message;

		public DummyMessage(string message)
		{
			this.message = message;
		}
	}

	public class MessageSystem
	{
		[Test]
		public void SpeedTest()
		{
			const string text = "hello";
			const int iterations = 100;
			GlobalEvents.AddListener<DummyMessage>(DummyMessageReciever);

			DummyMessage message = new DummyMessage(text);


			TimeSpan[] timeSpans = new TimeSpan[iterations];
			Stopwatch stopwatch = null;
			for (int i = 0; i < iterations; i++)
			{
				stopwatch = Stopwatch.StartNew();
				for (int y = 0; y < 100000; y++)
				{
					GlobalEvents.SendMessage(message);
				}
				stopwatch.Stop();
				timeSpans[i] = stopwatch.Elapsed;
			}
			GlobalEvents.RemoveListener<DummyMessage>(DummyMessageReciever);
			Assert.Pass($"MessageSystem Speedtest: {timeSpans.Average().Milliseconds}ms");
		}

		[Test]
		public void ReliabilityTest()
		{
			int count = 0;
			Action<DummyMessage> reciever1 = x => { count++; };
			Action<DummyMessage> reciever2 = x => { count++; };
			Action<DummyMessage> reciever3 = x => { count++; };
			GlobalEvents.AddListener(reciever1);
			GlobalEvents.AddListener(reciever2);
			GlobalEvents.AddListener(reciever3);

			GlobalEvents.SendMessage(new DummyMessage(""));

			GlobalEvents.RemoveListener(reciever1);
			GlobalEvents.RemoveListener(reciever2);
			GlobalEvents.RemoveListener(reciever3);

			Assert.AreEqual(3, count, "Not all three messages were received");

		}

		private void DummyMessageReciever(DummyMessage x)
		{

		}
	}
}
