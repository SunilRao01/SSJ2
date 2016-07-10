var launcher = function(game)
{
	console.log("%cStarting SSJ2..", "color:white; background:black");
};

launcher.prototype = 
{
	preload: function()
	{
		this.game.load.image("loading","assets/loading_bar.png"); 
	},

	create: function()
	{
		this.game.state.start("Preload");
	}
}