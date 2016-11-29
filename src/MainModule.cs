using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Nancy.Simple
{
	public class MainModule : NancyModule
	{
		public MainModule ()
		{
			Get ["/"] = _ => {
				var contentBytes = Encoding.UTF8.GetBytes ("OK");
				var response = new Response {
					ContentType = "text/plain",
					Contents = s => s.Write (contentBytes, 0, contentBytes.Length),
					StatusCode = HttpStatusCode.OK
				};
				return response;
			};

            Post ["/"] = parameters => {
				var form = Request.Form;
				string action = form ["action"];
				switch (action) {
				case "bet_request":
				{
                    TestConnectionToRainMan();
                    return PerformBet2(form);
				}
				case "showdown":
				{
					var json = JObject.Parse (form ["game_state"]);
					PokerPlayer.ShowDown (json);
					var showDownBytes = Encoding.UTF8.GetBytes ("OK");
					var response = new Response {
						ContentType = "text/plain",
						Contents = s => s.Write (showDownBytes, 0, showDownBytes.Length),
						StatusCode = HttpStatusCode.OK
					};
					return response;
				}
				case "version":
				{
					var versionBytes = Encoding.UTF8.GetBytes (PokerPlayer.VERSION);
					return new Response {
						ContentType = "text/plain",
						Contents = s => s.Write (versionBytes, 0, versionBytes.Length),
						StatusCode = HttpStatusCode.OK
					};
				}
				case "check":
				{
					var contentBytes = Encoding.UTF8.GetBytes ("OK");
					var response = new Response {
						ContentType = "text/plain",
						Contents = s => s.Write (contentBytes, 0, contentBytes.Length),
						StatusCode = HttpStatusCode.OK
					};
					return response;
				}
				default:
					var bytes = Encoding.UTF8.GetBytes ("Not an allowed action or request");
					return new Response {
						ContentType = "text/plain",
						Contents = s => s.Write (bytes, 0, bytes.Length),
						StatusCode = HttpStatusCode.BadRequest
					};
				}
			};
		}

        private static void TestConnectionToRainMan()
        {
            string rainManURI = "http://rainman.leanpoker.org/rank";
            var WebClient = new WebClient();

            var rainManSampleJson = @"{ ""rank"":""5"", ""suit"":""diamonds"" }";

            //http://stackoverflow.com/questions/15091300/posting-json-to-url-via-webclient-in-c-sharp

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string rainManResponse = cli.UploadString(rainManURI, rainManSampleJson);
            Console.Error.WriteLine(rainManResponse);
        }


        private static Response PerformBet2(dynamic form)
        {
            var json = JObject.Parse(form["game_state"]);
            GameState gameState = json.ToObject<GameState>();
            Console.Error.WriteLine("My bet index: " + gameState.bet_index);

            var bet = PokerPlayer.BetRequest(gameState).ToString();
            var betBytes = Encoding.UTF8.GetBytes(bet);
            var response = new Response
            {
                ContentType = "text/plain",
                Contents = s => s.Write(betBytes, 0, betBytes.Length),
                StatusCode = HttpStatusCode.OK
            };
            return response;
        }

        private static Response PerformBet(dynamic form)
	    {
	        var json = JObject.Parse(form["game_state"]);
	        GameState gameState = json.ToObject<GameState>();
	        Console.Error.WriteLine("My bet index: " + gameState.bet_index);

	        var bet = PokerPlayer.BetRequest(json).ToString();
	        var betBytes = Encoding.UTF8.GetBytes(bet);
	        var response = new Response
	        {
	            ContentType = "text/plain",
	            Contents = s => s.Write(betBytes, 0, betBytes.Length),
	            StatusCode = HttpStatusCode.OK
	        };
	        return response;
	    }
	}
}
