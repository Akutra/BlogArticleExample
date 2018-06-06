//This will need a little revision to actually run, it is meant to be a basic overview

///
/// Example of a simple Node.js Server that could serve REST services
/// Akutra R.A> Cea

var http = require("http");
var url = require('url');
var fs = require('fs');

var server = http.createServer(function(request, response){
	var path = url.parse(request.url).pathname;

	switch(path.substring(0,3)){
		case '/get':
			//....response write...
			break;
		case '/put':
			//....response write...
			break;
		default:
			response.writeHead(404);
			response.write("404");
			response.end();
			break;
	}
});

server.listen(8001);