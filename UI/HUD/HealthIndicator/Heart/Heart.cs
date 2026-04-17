using Godot;
using System;

namespace UI
{
    public partial class Heart : MarginContainer
    {
        [Export]
        public bool IsActive
        {
            get;
            set
            {
                field = value;
                ((ShaderMaterial)Material).SetShaderParameter(IsActiveParam, !field);
            }
        } = true;

        // Matches the 'uniform' param in the shader
        private const string IsActiveParam = "is_active";
    }
}
