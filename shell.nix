{ pkgs ? import <nixpkgs> {} }:
pkgs.mkShell {
  buildInputs = with pkgs; [
    dotnet-sdk_10 # ili 9, zavisno šta koristiš
    postgresql_16
    dotnet-ef
  ];

  shellHook = ''
    export PGDATA="$PWD/db_data"
    if [ ! -d "$PGDATA" ]; then
      initdb -D "$PGDATA" --auth=trust --no-local
    fi
    # Pokreće postgres na portu 5432 u pozadini
    pg_ctl -D "$PGDATA" -l logfile -o "-k /tmp" start
  '';
}

#     dotnet ef migrations add DodatiNoviModeli
#     dotnet ef database update
