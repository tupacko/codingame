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
while true; do
    # count: The number of current enemy ships within range
    read count
    cD=99999
    for (( i=0; i<count; i++ )); do
        # enemy: The name of this enemy
        # dist: The distance to your cannon of this enemy
        read enemy dist
        if ((cD>dist)); then
            cD=$dist
            cE=$enemy
        fi
    done

    # Write an action using echo
    # To debug: echo "Debug messages..." >&2

    echo $cE # The name of the most threatening enemy (HotDroid is just one example)
done