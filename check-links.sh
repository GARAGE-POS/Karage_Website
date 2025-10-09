#!/bin/bash
# Script to build Hugo site and check for broken links and images

echo "Building Hugo site..."
hugo --cleanDestinationDir

echo "Checking for broken links and images..."
linkchecker public/

echo "Done."