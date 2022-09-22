# ReedScraper

The idea for this project came up when I realised no popular job hunting sites had the ability to prevent results containing certain keywords from appearing.
The program in it's current state takes a preset list of words and searches for them in the job listings' descriptions, then returns only jobs which do *not* contain said words in their descriptions, to a text file named 'Output.txt'.

This was just something I threw together in a couple of hours to solve a problem I had. The code hasn't been refactored at all and is difficult to read. Major functionality is obviously missing.

### TODO
* Needs multipage support implementing.
* Needs major code revisions to improve scalability/readability.
* Additional user controls to control queries.
