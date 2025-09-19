{{/*
Expand the name of the chart.
*/}}
{{- define "thesis.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "thesis.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "thesis.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "thesis.labels" -}}
helm.sh/chart: {{ include "thesis.chart" . }}
{{ include "thesis.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- with .Values.commonLabels }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "thesis.selectorLabels" -}}
app.kubernetes.io/name: {{ include "thesis.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "thesis.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "thesis.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Docker registry prefix
*/}}
{{- define "thesis.imageRegistry" -}}
{{- if .Values.global.imageRegistry }}
{{- printf "%s/" .Values.global.imageRegistry }}
{{- else if .Values.dockerRegistry }}
{{- printf "%s/" .Values.dockerRegistry }}
{{- end }}
{{- end }}

{{/*
Get the image name for a service
*/}}
{{- define "thesis.image" -}}
{{- $registry := include "thesis.imageRegistry" . }}
{{- $repository := .image.repository }}
{{- $tag := .image.tag | default "latest" }}
{{- printf "%s%s:%s" $registry $repository $tag }}
{{- end }}

{{/*
Common labels for services
*/}}
{{- define "thesis.serviceLabels" -}}
{{ include "thesis.labels" . }}
{{- with .labels }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Common selector labels for services
*/}}
{{- define "thesis.serviceSelectorLabels" -}}
{{ include "thesis.selectorLabels" . }}
app.kubernetes.io/component: {{ .name }}
{{- end }}

{{/*
Environment variables for all services
*/}}
{{- define "thesis.envVars" -}}
{{- range .env }}
- name: {{ .name }}
  value: {{ .value | quote }}
{{- end }}
{{- end }}

{{/*
Security context
*/}}
{{- define "thesis.securityContext" -}}
{{- with .Values.securityContext }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Pod security context
*/}}
{{- define "thesis.podSecurityContext" -}}
{{- with .Values.podSecurityContext }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Resource limits and requests
*/}}
{{- define "thesis.resources" -}}
{{- with .resources }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Node selector
*/}}
{{- define "thesis.nodeSelector" -}}
{{- with .nodeSelector }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Tolerations
*/}}
{{- define "thesis.tolerations" -}}
{{- with .tolerations }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Affinity
*/}}
{{- define "thesis.affinity" -}}
{{- with .affinity }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
Ingress annotations
*/}}
{{- define "thesis.ingressAnnotations" -}}
{{- with .ingress.annotations }}
{{ toYaml . }}
{{- end }}
{{- end }}

{{/*
PostgreSQL connection string
*/}}
{{- define "thesis.postgresqlConnectionString" -}}
{{- if .Values.postgresql.enabled }}
{{- printf "Host=%s-postgresql;Database=%s;Username=%s;Password=%s" .Release.Name .Values.postgresql.auth.database .Values.postgresql.auth.username .Values.postgresql.auth.password }}
{{- else }}
{{- "Host=postgresql;Database=thesis;Username=postgres;Password=postgres" }}
{{- end }}
{{- end }}

{{/*
Redis connection string
*/}}
{{- define "thesis.redisConnectionString" -}}
{{- if .Values.redis.enabled }}
{{- printf "%s-redis-master:6379" .Release.Name }}
{{- else }}
{{- "redis:6379" }}
{{- end }}
{{- end }}

{{/*
RabbitMQ connection string
*/}}
{{- define "thesis.rabbitmqConnectionString" -}}
{{- if .Values.rabbitmq.enabled }}
{{- printf "amqp://%s:%s@%s-rabbitmq:5672/" .Values.rabbitmq.auth.username .Values.rabbitmq.auth.password .Release.Name }}
{{- else }}
{{- "amqp://guest:guest@rabbitmq:5672/" }}
{{- end }}
{{- end }}

{{/*
Seq endpoint
*/}}
{{- define "thesis.seqEndpoint" -}}
{{- if .Values.seq.enabled }}
{{- printf "http://%s-seq:5341" .Release.Name }}
{{- else }}
{{- "http://seq:5341" }}
{{- end }}
{{- end }}