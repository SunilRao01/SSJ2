var preload = function(game) {}

preload.prototype = 
{
	preload: function()
	{
		// Loading bar
		var loadingBar = this.add.sprite(160, 240, "loading");
		loadingBar.anchor.setTo(0.5, 0.5);
		this.load.setPreloadSprite(loadingBar);

		// Temp start screen clouds
		this.game.load.image('cloud_1', 'assets/cloud_1.png')
		this.game.load.image('cloud_2', 'assets/cloud_2.png')
		this.game.load.image('cloud_3', 'assets/cloud_3.png')
		this.game.load.image('cloud_4', 'assets/cloud_4.png')
		/*this.game.load.image('bg', 'assets/bg.jpg');
		this.game.load.spritesheet('horses', 'assets/horse/horse_spritesheet.png?v=5', 184, 117, 11);
		this.game.load.image('bg', 'assets/bg.jpg');
		this.game.load.image('dice', 'assets/dice_ui.png');
		this.game.load.image('finish', 'assets/finish_line.png');
		this.game.load.image('horse', 'assets/horse/standing_horse.png?v=1');*/
	},

	create: function()
	{
		this.game.state.start("StartMenu");
	}
}