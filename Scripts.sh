#!/bin/bash

################################################## variables ##################################################
postgres_image="postgis/postgis:16-3.5" # "postgis/postgis:17-master"
postgres_container="postgres"
postgres_user="postgres"
postgres_password="P@ssw0rd"
dbuser_user="dbuser"
dbuser_password="P@ssw0rd"

################################################## functions ##################################################
solution_initilize(){
  project=$1
  dotnet tool restore
  dotnet user-secrets set "postgres_credential" "User Id=$dbuser_user;Password=$dbuser_password" --project $project # set secrets
}

update_database(){
  project=$1
  startup_project=$2
  rm -rf $project/Migrations # remove migration folder
  # dotnet build
  dotnet ef migrations add Initial --project $project --startup-project $startup_project # create initial migration
  dotnet ef migrations script --project $project --startup-project $startup_project --output $project/Migrations/Script.sql # create script
  dotnet ef database drop --project $project --startup-project $startup_project --force # drop database
  dotnet ef database update --project $project --startup-project $startup_project # update database
}

run_test() {
  project=$1
  rm -rf $project/TestResults
  dotnet test $project --settings $project/coverlet.runsettings --collect:"XPlat Code Coverage"
  dotnet reportgenerator -reports:"$project/TestResults/*/coverage.cobertura.xml" -targetdir:"$project/TestResults/CoverageReport" -reporttypes:Html
  # google-chrome $project/TestResults/CoverageReport/index.html &
  # /opt/microsoft/msedge/msedge $project/TestResults/CoverageReport/index.html &
}

################################################## execute ##################################################
clear

# solution_initilize "Notification"
# update_database "Notification" "Notification"
run_test "Notification.Tests"
