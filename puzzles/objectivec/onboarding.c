#include <Foundation/Foundation.h>

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
int main(int argc, const char * argv[]) {

    // game loop
    while (1) {
        int count; // The number of current enemy ships within range
        scanf("%d", &count); fgetc(stdin);
        char cE[50];
        int cD = 99999;
        for (int i = 0; i < count; i++) {
            char enemy[50]; // The name of this enemy
            int dist; // The distance to your cannon of this enemy
            scanf("%s%d", enemy, &dist); fgetc(stdin);
            if (cD > dist)
            {
                cD = dist;
                strcpy(cE, enemy);
            }
        }

        // Write an action using printf(). DON'T FORGET THE TRAILING NEWLINE \n
        // To debug: fprintf(stderr, [@"Debug messages\n" UTF8String]);

        printf([@"%s\n" UTF8String], cE); // The name of the most threatening enemy (HotDroid is just one example)
    }
}