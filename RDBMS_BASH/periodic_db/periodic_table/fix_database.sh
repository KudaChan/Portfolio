#!/bin/bash

PSQL="psql -X --username=freecodecamp --dbname=periodic_table -t --no-align -c"

# : "
# SCRIPT TO ADD type_id INTO properties TABLE
# "

# TYPE_PROPERTY=$($PSQL "SELECT type FROM properties ORDER BY atomic_number")
# echo "$TYPE_PROPERTY" | while read TYPE
# do
#   TYPE_ID=$($PSQL "SELECT type_id FROM types WHERE type = '$TYPE'")
#   if [[ ! -z $TYPE_ID ]]
#   then
#     echo $($PSQL "UPDATE properties SET type_id = $TYPE_ID WHERE type = '$TYPE'")
#   else
#     echo 'error: $TYPE_ID is null'
#   fi
# done

: "
SCRIPT TO CHANGE FIRST LETTER of symbol TO UPPERCASE
"
SYMBOL=$($PSQL "SELECT symbol FROM elements")
echo "$SYMBOL" | while read SYMBOL_NAME
do
  NEW_SYMBOL="$(echo "$SYMBOL_NAME" | sed 's/^\(.\)/\U\1/')"
  echo $($PSQL "UPDATE elements SET symbol = '$NEW_SYMBOL' WHERE symbol = '$SYMBOL_NAME'")
done
