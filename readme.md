# DbUp-DIY-CLI [![Build status][av-badge]][build] [![NuGet version][nuget-badge]][nuget-link]

Do-It-Yourself CommandLine Interface for DbUp

## Introduction

dbup is a great library but requires some repeatable setup in code:

1. Where are the scripts do run?
1. Where is the destination database?
1. How do I drop and recreate the database?
1. Can I optionally run some seed data for developers/deployment?

## Usage

### Setup

1. Create a Console project (like `ConsoleApplication1`)
1. `Install-Package dbup-diy-cli`
1. Add your SQL files as embedded resources
1. Modify your `Main`:

    public static int Main(string[] args)
    {
        return new DbUp.Cli.Upgrader(args).Run();
    }
1. (optional) install db-specific package like `dbup-mysql`, `dbup-sqlite`, etc

### Running
    
To upgrade a local instance (uses Windows auth) run `ConsoleApplication1.exe local`
    
```
-d, --database            Required. Database to upgrade

-s, --server              Required. Server name/address

--dev-seeds               Seed database with sample data

-r, --recreate            Completely recreate all database objects

-m, --mark-as-executed    Don't run ever run current migration scripts

--dev-seed-pattern        (Default: _dev_) Regular expression to match and
                          select developer seed script (case-insensitive)

--help                    Display this help screen.

--version                 Display version information.
```

To upgrade an arbitrary server, use `ConsoleApplication1.exe remote`, with which you can provide a complete 
connectionstring (`-c` switch) or a name to find in `app.config` (`-n` switch)

```
-c, --connection-string         connection string

-n, --connection-string-name    connection string name

--dev-seeds                     Seed database with sample data

-r, --recreate                  Completely recreate all database objects

-m, --mark-as-executed          Don't run ever run current migration scripts

--dev-seed-pattern              (Default: _dev_) Regular expression to match
                                and select developer seed script (case-insensitive)

--help                          Display this help screen.

--version                       Display version information.
```

## Why not a ready-built CLI?

This way it's easier to use with database engines other that MS SQL, by installing `dbup-X` package together with 
`dbup-diy-cli`.

[av-badge]: https://ci.appveyor.com/api/projects/status/hvhilv1f12n6dl52/branch/master?svg=true
[build]: https://ci.appveyor.com/project/tpluscode78631/dbup-diy-cli/branch/master
[nuget-badge]: https://badge.fury.io/nu/dbup-diy-cli.svg
[nuget-link]: https://badge.fury.io/nu/dbup-diy-cli
