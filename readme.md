# Plexure Samples

## Excercise 1

Can be found in DownloadClient.cs

## Excercise 2

Can be found in Models.Coupon.cs

## Excercise 3

Can be found in Plexure.Sample.Tests project

## Excercise 4

### 1. ItineraryManager class

Issues

    ItineraryManager doesn't implement an interface. 

	This means services using this class would breach the D in the SOLID principles. "Rely on abstractions not concrete implementations"

	Makes mocking messy, better to mock an interface

	More code changes requried to swap out the ItineraryManager for another implementation

Fixes

    Create an interface for the class

### 2. ItineraryManager Constructor

Issues

    Newing up classes in the constructor

	Unit testing becomes hard as you can't mock out the datastore or the distance calculator

	When you want to use a different type of datastore or distance calculator you have to make more changes

Fix

    Use interfaces for the datastore and distance calculator
    
    Use DI to configure these

### 3. _dataStore.GetItinaryAsync(itineraryId).Result;

Issues
	
    Cause the application to hang, waiting for the GetItinaryAsync to complete before moving on.

Fix

    Use await and change the return type async Task<IEnumerable<....

### 4. priceProviders parameter

Issues

    No null check on the priceProviders parameter

Fix

    Add a null check

### 5. if (itinerary == null)

Issue

    throw new InvalidOperationException();

Fix

    You could return an empty quote list instead of throwing an exception (Preference/Contact thing). I don't know what the contract is on the function. Should the system have gotten this far if the itinerary was invalid?


### 6. Parallel.ForEach(priceProviders, provider =>

Issues

    Trying to access local variable from a different thread

Fix

    You could use concurrentbag or something similar

### 7. CalculateTotalTravelDistanceAsync

Issues

    Result, same as issue 3
    
    itinerary.Waypoints[i + 1] will throw an out of index error
    
    result = result + could be changed

Fix

    Change the loop to store the last origin value and use that to calculate the distance between it and the destination. If origin isn't set, set it and continue as there is no destination at this point to calculate the distance. 
    
    result +=
