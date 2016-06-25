# Auto-generated code below aims at helping you parse
# the standard input according to the problem statement.

read N
readarray -n $N powers
powers=($(for each in ${powers[@]}; do echo $each; done | sort -n))

minDiff=9999999999
minDiffCount=0
max=$((N - 1))
for (( i=0; i<max && minDiffCount < 5000; i++)); do
    crtDiff=$((${powers[i+1]}-${powers[i]}))
    if [ $minDiff -gt $crtDiff ]
    then
        minDiff=${crtDiff}
        minDiffCount=0
    fi
    
    minDiffCount=$(($minDiffCount + 1))
done

echo ${minDiff}