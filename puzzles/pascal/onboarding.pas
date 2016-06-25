// The code below will read all the game information for you.
// On each game turn, information will be available on the standard input, you will be sent:
// -> the total number of visible enemies
// -> for each enemy, its name and distance from you
// The system will wait for you to write an enemy name on the standard output.
// Once you have designated a target:
// -> the cannon will shoot
// -> the enemies will move
// -> new info will be available for you to read on the standard input.
program Answer;
{$H+}
uses sysutils, classes, math;

// Helper to read a line and split tokens
procedure ParseIn(Inputs: TStrings) ;
var Line : string;
begin
    readln(Line);
    Inputs.Clear;
    Inputs.Delimiter := ' ';
    Inputs.DelimitedText := Line;
end;

var
    count : Int32; // The number of current enemy ships within range
    enemy : String; // The name of this enemy
    dist : Int32; // The distance to your cannon of this enemy
    i : Int32;
    Inputs: TStringList;
    cE : String;
    cD : Int32;
begin
    Inputs := TStringList.Create;

    // game loop
    while true do
    begin
        cD := 99999;
    
        ParseIn(Inputs);
        count := StrToInt(Inputs[0]);
        for i := 0 to count-1 do
        begin
            ParseIn(Inputs);
            enemy := Inputs[0];
            dist := StrToInt(Inputs[1]);
            if cD > dist
            then
            begin
                cD := dist;
                cE := enemy;
            end;
        end;

        // Write an action using writeln()
        // To debug: writeln(StdErr, 'Debug messages...');

        writeln(cE); // The name of the most threatening enemy (HotDroid is just one example)
        flush(StdErr); flush(output); // DO NOT REMOVE
    end;
end.