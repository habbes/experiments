# OOP Demo

The sample app imports data from various sources into a single database/repository.

Scenario:

We want to create an app that allows the user to query information about
products. We are pulling product information from different data sources,
which may have different formats. This data should be presented
to the user in a consistent uniform format, the user doesn't care
whether product A is pulled from a different data sources as product B.
The different data sources could be different types of files, APIs,
data scraped from websites, etc. From each of those sources we
can extract a list of products that we can present to the user.
We can assume that each source allows us to extract some common info
like product name, price, description, etc.
