var startMenu = function(game) {}

WebFontConfig = {

    //  'active' means all requested fonts have finished loading
    //  We set a 1 second delay before calling 'createText'.
    //  For some reason if we don't the browser cannot render the text the first time it's created.


    //  The Google Fonts we want to load (specify as many as you like in the array)
    google: {
      families: ['Bungee Shade']
    }

};

var movingHorse;
var cloudMovementFlag = 0;
var clouds;
startMenu.prototype = 
{
	

	playTheGame: function()
	{
		this.game.state.start("Sandbox");
	},
	preload: function()
	{
		this.load.script('webfont', '//ajax.googleapis.com/ajax/libs/webfont/1.4.7/webfont.js');
	},
	
	create: function()
	{
		// Background color
		this.stage.backgroundColor = '#81c9f9';

		// Text
		var title = this.game.add.text(this.game.world.centerX, this.game.world.centerY-160, "Summer Slow Jam");
		title.font = 'Bungee Shade';
		title.fontSize = 50;
		var startButton = this.game.add.button(this.game.world.centerX, this.game.world.centerY+160, null, this.sandbox, this);
		startButton.width = 100;

		title.anchor.set(0.5);
		startButton.anchor.set(0.5);

		title.fill = '#ffffff'
		title.stroke = '#000000';
    	title.strokeThickness = 2;

    	clouds = this.add.group();

    	// Add 1 pre exisitng clouds
    	var random = Math.floor(Math.random() * (4 - 1 + 1)) + 1;
    	var cloudPosition = new Phaser.Point(0, Math.floor(Math.random() * (210 - 100 + 100)) + 100);
    	
    	switch (random)
    	{
    		case 1:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_1');

    			break;
    		case 2:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_2');

    			break;
    		case 3:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_3');

    			break;
    		case 4:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_4');

    			break;
    		default:
				throw "ERROR: Couldn't load cloud!"
    			break;
    	}

    	var c = clouds.getAt(clouds.length-1);
		c.x -= clouds.getAt(clouds.length-1).width;
		clouds.replace(clouds.getAt(clouds.length-1), c);
		

    	// Constantly add clouds every 3 seconds
    	this.time.events.loop(Phaser.Timer.SECOND * 5, this.spawnCloud, this);

    	//this.spawncloud();
		// Animated horse
		/*var horses = this.game.add.sprite(this.game.world.centerX - 92, this.game.world.centerY - 58.5, 'horses');
		horses.tint = 0x00;
		horses.animations.add('move');

		horses.animations.play('move', 20, true);
		movingHorse = horses;*/
	},
	spawnCloud: function()
	{
		var random = Math.floor(Math.random() * (4 - 1 + 1)) + 1;
    	var cloudPosition = new Phaser.Point(0, Math.floor(Math.random() * (210 - 100 + 100)) + 100);
    	
    	switch (random)
    	{
    		case 1:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_1');

    			break;
    		case 2:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_2');

    			break;
    		case 3:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_3');

    			break;
    		case 4:
    			clouds.create(cloudPosition.x, cloudPosition.y, 'cloud_4');

    			break;
    		default:
				throw "ERROR: Couldn't load cloud!"
    			break;
    	}

    	var c = clouds.getAt(clouds.length-1);
		c.x -= clouds.getAt(clouds.length-1).width;
		clouds.replace(clouds.getAt(clouds.length-1), c);
	},
	update: function()
	{
		if (clouds.length > 0)
		{
			clouds.forEach(function(cloud){ cloud.x += 1; }, this);;

			var toDestroy = clouds.filter(function(cloud) { return cloud.x > 640; });
			toDestroy.callAll('destroy');
		}
	}

	
}