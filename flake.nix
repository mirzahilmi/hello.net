{
  description = "A Nix-flake-based C# development environment";

  inputs.nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";

  outputs = {
    self,
    nixpkgs,
  }: let
    supportedSystems = ["x86_64-linux" "aarch64-linux" "x86_64-darwin" "aarch64-darwin"];
    forEachSupportedSystem = f:
      nixpkgs.lib.genAttrs supportedSystems (system:
        f {
          pkgs = import nixpkgs {
            inherit system;
            config.permittedInsecurePackages = [
              "dotnet-wrapped-combined"
              "dotnet-combined"
              "dotnet-sdk-wrapped-6.0.428"
              "dotnet-sdk-6.0.428"
            ];
          };
        });
  in {
    devShells = forEachSupportedSystem ({pkgs}: {
      default = pkgs.mkShell {
        packages = with pkgs; [
          netcoredbg
          csharpier
          roslyn-ls
          (with dotnetCorePackages;
            combinePackages [
              sdk_6_0
              sdk_8_0
            ])
        ];
      };
    });
  };
}
