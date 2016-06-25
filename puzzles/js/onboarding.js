/**
 * The code below will read all the game information for you.
 * On each game turn, information will be available on the standard input, you will be sent:
 * -> the total number of visible enemies
 * -> for each enemy, its name and distance from you
 * The system will wait for you to write an enemy name on the standard output.
 * Once you have designated a target:
 * -> the cannon will shoot
 * -> the enemies will move
 * -> new info will be available for you to read on the standard input.
 **/

function getEnemy()
{
    var inputs = readline().split(' ');
    return {
        name: inputs[0],
        distance: parseInt(inputs[1])
    }
}

function getCloser(x, y)
{
    if (x.distance < y.distance)
    {
        return x;
    }
    
    return y;
}

// game loop
while (true) {
    var count = parseInt(readline()); // The number of current enemy ships within range
    var closest = {name : '', distance : 100000};
    for (var i = 0; i < count; i++) {
        var current = getEnemy();
        var closest = getCloser(closest, current);
    }

    // Write an action using print()
    // To debug: printErr('Debug messages...');

    print(closest.name); // The name of the most threatening enemy (HotDroid is just one example)
}