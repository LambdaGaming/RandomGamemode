# Contributing to this repository
Please read below before creating issues or pull requests.

## Issues
- Issues can be in any format you'd like as long as it's readable and in English.
- Avoid making duplicate issues. Check to make sure the issue you want to report hasn't been reported already.
- Verify that this plugin is responsible for your issue before reporting.

## Pull Requests
- Try to keep your pull requests small and directed towards a single change. If you want to change or add multiple unrelated things, make separate pull requests for each of them.
- Complete rewrites of large parts and extremely small changes probably won't be accepted unless you can provide proof that the change greatly improves performance and/or functionality.
	### Styling
	- Styling won't be strictly enforced unless it looks unreadable compared to the rest of the code. If you choose not to follow one or two of the styling rules shown below you'll still be fine.
	- Use size 4 tabs instead of spaces. I know this is unconventional but I find it to be neater.
	- See this example for how your code should be styled:
		``` csharp
			// Comments should have a space in front of the double forward slashes
			// Placing comments in your actual code is recommended but not required
			public bool Equals( string text1, string text2 ) // Put spaces in between the parenthesis and the arguments as well as after commas
			{ // Bring the bracket down for methods and classes
				string[] blacklist = { // keep the bracket here for arrays, lists, dictionaries, etc
					"word",
					"example",
					"blacklist" // Make a new line for each value in an array unless they start to take up too much room, then put them all on a single line
				};
				List<string> newlist = new List<string>(); // Don't put spaces inside angle brackets, square brackets, or empty parenthesis or brackets
				newlist.Add( blacklist[0] );

				// Leave whitespace in between control structures and variable declarations
				foreach ( string word in newlist )
				{
					if ( word == text1 )
					{ // Use braces and tabs for multiple lines inside control structures
						Console.WriteLine( "Word is blacklisted. Aborting." );
						return false;
					}
				}

				if ( text1 == text2 )
					return true; // For single lines inside control structures, use a tab without braces
				return false;
			}
		```
