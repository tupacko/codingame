#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <limits.h>

typedef struct enemy_type
{
    char name[50];
    int distance;
} enemy;

enemy readEnemy()
{
    char name[50];
    int distance;
    scanf("%s%d", name, &distance); fgetc(stdin);
    //fprintf(stderr, "Found enemy: %s %d\n", name, distance);
    
    enemy current;
    strcpy(current.name, name);
    current.distance = distance;
    
    return current;
}

int getNumberOfEnemies()
{
    int count;
    scanf("%d", &count); fgetc(stdin);
    
    return count;
}

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
int main()
{

    // game loop
    while (1) {
        //fprintf(stderr, "Begin loop ...\n");
        int count = getNumberOfEnemies();
        //fprintf(stderr, "Enemies count is %d\n", count);
        
        enemy closest = { .distance = INT_MAX };
        //fprintf(stderr, "Closest enemy init to %d\n", closest.distance);
        for (int i = 0; i < count; i++) {
            //fprintf(stderr, "Reading current enemy ...\n");
            enemy current = readEnemy();
            //fprintf(stderr, "Current enemy read\n");
            if (closest.distance > current.distance)
            {
                //fprintf(stderr, "Closer enemy found\n");
                closest = current;
            }
        }

        // Write an action using printf(). DON'T FORGET THE TRAILING \n
        // To debug: fprintf(stderr, "Debug messages...\n");
        //fprintf(stderr, "Closest enemy found: %s %d\n", closest.name, closest.distance);
        
        printf("%s\n", closest.name); // The name of the most threatening enemy (HotDroid is just one example)
        //fprintf(stderr, "End loop ...\n");
    }
}