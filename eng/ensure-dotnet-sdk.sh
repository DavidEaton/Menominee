#!/usr/bin/env bash
set -euo pipefail

# Installs the .NET SDK required by global.json when the host image does not
# already provide a compatible dotnet executable. This is intentionally local to
# the repository by default so it can be used in ephemeral agent containers.

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
install_dir="${DOTNET_INSTALL_DIR:-$repo_root/.dotnet}"
global_json="$repo_root/global.json"

if command -v dotnet >/dev/null 2>&1; then
    if dotnet --list-sdks | awk '{print $1}' | grep -Eq '^10\.0\.'; then
        dotnet --info
        exit 0
    fi
fi

sdk_version="$(python3 - <<'PY' "$global_json"
import json, sys
with open(sys.argv[1], encoding='utf-8') as f:
    print(json.load(f)['sdk']['version'])
PY
)"

mkdir -p "$install_dir"
install_script="$(mktemp)"
trap 'rm -f "$install_script"' EXIT

if ! curl -fsSL "${DOTNET_INSTALL_SCRIPT_URL:-https://dot.net/v1/dotnet-install.sh}" -o "$install_script"; then
    cat >&2 <<ERR
Unable to download dotnet-install.sh. Check network/proxy access to https://dot.net,
or set DOTNET_INSTALL_SCRIPT_URL to a reachable mirror of the official install script.
ERR
    exit 1
fi

bash "$install_script" --version "$sdk_version" --install-dir "$install_dir" --no-path

cat <<MSG
.NET SDK $sdk_version installed to $install_dir.
For this shell, run:
  export DOTNET_ROOT="$install_dir"
  export PATH="$install_dir:\$PATH"
MSG

"$install_dir/dotnet" --info
