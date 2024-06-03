#!/bin/bash

PSQL="psql --username=freecodecamp --dbname=number_guess -t --no-align -c"

GAME_FUNC()
{
  echo "Enter your username:"
  read NAME
  n=${#NAME}

  if [[ ! $n -lt 22 || ! $n -gt 0 ]]
  then
    GAME_FUNC
  else
    USER_NAME=$(echo $($PSQL "SELECT name FROM users WHERE name = '$NAME'") | sed 's/ //g')

    if [[ ! -z $USER_NAME ]]
    then
      USER_ID=$(echo $($PSQL "SELECT user_id FROM users WHERE name = '$USER_NAME'") | sed 's/ //g')
      USER_NAME=$(echo $($PSQL "SELECT name FROM users WHERE user_id = $USER_ID") | sed 's/ //g')
      GAME_PLAYED=$(echo $($PSQL "SELECT game_played FROM users WHERE user_id = $USER_ID") | sed 's/ //g')
      BEST_SCORE=$(echo $($PSQL "SELECT MIN(score) FROM users LEFT JOIN games USING(user_id) WHERE user_id = $USER_ID") | sed 's/ //g')
      echo "Welcome back, $USER_NAME! You have played $GAME_PLAYED games, and your best game took $BEST_SCORE guesses."
    else
      USER_NAME=$NAME
      echo -e "Welcome, $USER_NAME! It looks like this is your first time here."
    fi
    CORRECT_ANSWER=$(( $RANDOM % 1000 + 1 ))
    TOTAL_GUESS=0
    GUESSER $USER_NAME $CORRECT_ANSWER $TOTAL_GUESS
  fi
}

GUESSER()
{
  USER_NAME=$1
  CORRECT_ANSWER=$2
  GUESS_COUNT=$3
  GUESS_NO=$4

  if [[ -z $GUESS_NO ]]
  then
    echo "Guess the secret number between 1 and 1000:"
    read GUESS_NO
  else
    echo "That is not an integer, guess again:"
    read GUESS_NO
  fi

  GUESS_COUNT=$(( $GUESS_COUNT + 1 ))
  if [[ $GUESS_NO =~ ^[0-9]+$ ]]
  then
    CHECK_ANSWER $USER_NAME $CORRECT_ANSWER $GUESS_COUNT $GUESS_NO
  else
    GUESSER $USER_NAME $CORRECT_ANSWER $GUESS_COUNT $GUESS_NO
  fi
}

CHECK_ANSWER()
{
  USER_NAME=$1
  CORRECT_ANSWER=$2
  GUESS_COUNT=$3
  GUESS_NO=$4

  if [[ $GUESS_NO < $CORRECT_ANSWER ]]
  then
    echo "It's higher than that, guess again:"
    GUESS_COUNT=$(( $GUESS_COUNT + 1 ))
    read GUESS_NO
  elif [[ $GUESS_NO > $CORRECT_ANSWER ]]
  then
    echo "It's lower than that, guess again:"
      GUESS_COUNT=$(( $GUESS_COUNT + 1 ))
    read GUESS_NO
  else
    GUESS_COUNT=$GUESS_COUNT
  fi

  if [[ ! $GUESS_NO =~ ^[0-9]+$ ]]
  then
    GUESSER $USER_NAME $CORRECT_ANSWER $GUESS_COUNT $GUESS_NO
  elif [[ $GUESS_NO -ne $CORRECT_ANSWER ]]
  then
    CHECK_ANSWER $USER_NAME $CORRECT_ANSWER $GUESS_COUNT $GUESS_NO
  else
    SAVE_USER $USER_NAME $GUESS_COUNT
    NUMBER_OF_GUESSES=$GUESS_COUNT
    SECRET_NUMBER=$CORRECT_ANSWER
    echo "You guessed it in $NUMBER_OF_GUESSES tries. The secret number was $SECRET_NUMBER. Nice job!"
  fi
}

SAVE_USER()
{
  USER_NAME=$1
  GUESS_COUNT=$2

  NAME=$($PSQL "SELECT name FROM users WHERE name = '$USER_NAME'")
  if [[ -z $NAME ]]
  then
    INSERT=$($PSQL "INSERT INTO users(name, game_played) VALUES('$USER_NAME', 1)")
  else
    GAME_PLAYED=$(( $($PSQL "SELECT game_played FROM users WHERE name = '$USER_NAME'") + 1))
    UPDATE=$($PSQL "UPDATE users SET game_played = $GAME_PLAYED WHERE name = '$USER_NAME'")
  fi
  
  USER_ID=$($PSQL "SELECT user_id FROM users WHERE name = '$USER_NAME'")
  INSERT=$($PSQL "INSERT INTO games(user_id, score) VALUES($USER_ID, $GUESS_COUNT)")
}

GAME_FUNC