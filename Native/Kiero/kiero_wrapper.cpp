#include "Kiero/kiero.h"

extern "C" {

    int kiero_wrapper_init(int render_type)
    {
        return (int)kiero::init((kiero::RenderType::Enum)render_type);
    }

    void kiero_wrapper_shutdown()
    {
        kiero::shutdown();
    }

    void* kiero_wrapper_get_methods_table()
    {
        return kiero::getMethodsTable();
    }

    int kiero_wrapper_get_render_type()
    {
        return (int)kiero::getRenderType();
    }

}