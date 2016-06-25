select(STDOUT); $| = 1; # DO NOT REMOVE

# The code below will read all the game information for you.
# On each game turn, information will be available on the standard input, you will be sent:
# -> the total number of visible enemies
# -> for each enemy, its name and distance from you
# The system will wait for you to write an enemy name on the standard output.
# Once you have designated a target:
# -> the cannon will shoot
# -> the enemies will move
# -> new info will be available for you to read on the standard input.


# game loop
while (1) {
    chomp($count = <STDIN>); # The number of current enemy ships within range
    $cE = "";
    $cD = 99999;
    for(my $i=0; $i<$count; $i++) {
        # enemy: The name of this enemy
        # dist: The distance to your cannon of this enemy
        chomp($tokens=<STDIN>);
        ($enemy, $dist) = split(/ /,$tokens);
        if ($cD > $dist)
        {
            $cD = $dist;
            $cE = $enemy;
        }
    }
    
    # Write an action using print
    # To debug: print STDERR "Debug messages...\n";

    print $cE;
    print "\n"; # The name of the most threatening enemy (HotDroid is just one example)
}