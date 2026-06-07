import { ModRegistrar } from "cs2/modding";
import { PinItButton } from "mods/pinit-button";
import { PinItPanel } from "mods/pinit-panel";
import { PinItToolOptionsWrapper } from "mods/pinit-add-button";
import { VanillaComponentResolver } from "mods/VanillaComponentResolver/VanillaComponentResolver";

const register: ModRegistrar = (moduleRegistry) => {
    // Required before any VanillaComponentResolver.instance usage.
    VanillaComponentResolver.setRegistry(moduleRegistry);

    moduleRegistry.append("GameTopRight", PinItButton);
    moduleRegistry.append("Game", PinItPanel);

    // Inject into the tool options panel — same hook point as Anarchy.
    moduleRegistry.extend(
        "game-ui/game/components/tool-options/mouse-tool-options/mouse-tool-options.tsx",
        "MouseToolOptions",
        PinItToolOptionsWrapper
    );
};

export default register;
