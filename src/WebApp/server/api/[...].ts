import { joinURL } from "ufo";

export default defineEventHandler(async (event) => {
    const proxyUrl = useRuntimeConfig().bffProxyUrl as string;

    // replace /api/gateway with /api
    const path = event.path?.replace("/api/gateway", "/api") || ""; // /api/gateway/endpoint to /api/endpoint
    console.log("path", path);
    const target = joinURL(proxyUrl, path);
    console.log("target", target); 
    return proxyRequest(event, target)
})