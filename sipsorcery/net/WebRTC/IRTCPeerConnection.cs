﻿//-----------------------------------------------------------------------------
// Filename: IRTCPeerConnection.cs
//
// Description: Contains the interface definition for the RTCPeerConnection
// class as defined by the W3C WebRTC specification. Should be kept up to 
// date with:
// https://www.w3.org/TR/webrtc/#interface-definition
//
// History:
// 16 Mar 2020	Aaron Clauson	Created.
//
// License: 
// BSD 3-Clause "New" or "Revised" License, see included LICENSE.md file.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIPSorcery.Net
{

    public enum RTCSdpType
    {
        answer = 0,
        offer = 1,
        pranswer = 2,
        rollback = 3
    }

    public class RTCOfferOptions
    {
        /// <summary>
        /// Optional. The remote address that was used for signalling during the connection
        /// set up. For non-ICE RTP sessions this can be used to determine the best local
        /// IP address to use in an SDP offer/answer.
        /// </summary>
        //public IPAddress RemoteSignallingAddress;
    }

    /// <summary>
    /// Options for creating an SDP answer.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#dictionary-rtcofferansweroptions-members.
    /// </remarks>
    public class RTCAnswerOptions
    {
        // Note: At the time of writing there are no answer options in the WebRTC specification.
    }

    public class RTCSessionDescription
    {
        public RTCSdpType type;
        public SDP sdp;
    }

    /// <summary>
    /// The types of credentials for an ICE server.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#rtcicecredentialtype-enum.
    /// </remarks>
    public enum RTCIceCredentialType
    {
        password
    }

    /// <summary>
    /// Used to specify properties for a STUN or TURN server that can be used by an ICE agent.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#rtciceserver-dictionary.
    /// </remarks>
    public class RTCIceServer
    {
        public string urls;
        public string username;
        public RTCIceCredentialType credentialType;
    }

    /// <summary>
    /// Determines which ICE candidates can be used for a peer connection.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#dom-rtcicetransportpolicy.
    /// </remarks>
    public enum RTCIceTransportPolicy
    {
        relay,
        all
    }

    /// <summary>
    /// Affects which media tracks are negotiated if the remote end point is not bundle aware.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#dom-rtcbundlepolicy.
    /// </remarks>
    public enum RTCBundlePolicy
    {
        balanced,
        max_compat,
        max_bundle
    }

    /// <summary>
    /// The RTCP multiplex options for ICE candidates. This option is currently redundant
    /// since the single option means RTCP multiplexing MUST be available or the SDP negotiation
    /// will fail.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#dom-rtcrtcpmuxpolicy.
    /// </remarks>
    public enum RTCRtcpMuxPolicy
    {
        require
    }

    /// <summary>
    /// Represents a fingerprint of a certificate used to authenticate WebRTC communications.
    /// </summary>
    public class RTCDtlsFingerprint
    {
        /// <summary>
        /// One of the hash function algorithms defined in the 'Hash function Textual Names' registry.
        /// </summary>
        public string algorithm;

        /// <summary>
        /// The value of the certificate fingerprint in lowercase hex string as expressed utilizing 
        /// the syntax of 'fingerprint' in [RFC4572] Section 5.
        /// </summary>
        public string value;
    }

    /// <summary>
    /// Represents a certificate used to authenticate WebRTC communications.
    /// </summary>
    public class RTCCertificate
    {
        /// <summary>
        /// The expires attribute indicates the date and time in milliseconds relative to 1970-01-01T00:00:00Z 
        /// after which the certificate will be considered invalid by the browser.
        /// </summary>
        public DateTimeOffset expires;

        public string X_CertificatePath;
        public string X_KeyPath;
        public string X_Fingerprint;

        public List<RTCDtlsFingerprint> getFingerprints()
        {
            throw new NotImplementedException("RTCCertificate.getFingerprints");
        }
    }

    /// <summary>
    /// Defines the parameters to configure how a new RTCPeerConnection is created.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#rtcconfiguration-dictionary.
    /// </remarks>
    public class RTCConfiguration
    {
        public List<RTCIceServer> iceServers;
        public RTCIceTransportPolicy iceTransportPolicy;
        public RTCBundlePolicy bundlePolicy;
        public RTCRtcpMuxPolicy rtcpMuxPolicy;
        public List<RTCCertificate> certificates;

        /// <summary>
        /// Size of the pre-fetched ICE pool. Defaults to 0.
        /// </summary>
        public int iceCandidatePoolSize = 0;
    }

    /// <summary>
    /// Signalling states for a WebRTC peer connection.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate.
    /// </remarks>
    public enum RTCSignalingState
    {
        stable,
        have_local_offer,
        have_remote_offer,
        have_local_pranswer,
        have_remote_pranswer,
        closed
    }

    /// <summary>
    /// The states a peer connection transitions through.
    /// </summary>
    /// <remarks>
    /// As specified in https://www.w3.org/TR/webrtc/#rtcpeerconnectionstate-enum.
    /// </remarks>
    public enum RTCPeerConnectionState
    {
        closed,
        failed,
        disconnected,
        @new,
        connecting,
        connected
    }

    interface IRTCPeerConnection
    {
        //IRTCPeerConnection(RTCConfiguration configuration = null);
        Task<RTCSessionDescriptionInit> createOffer(RTCOfferOptions options = null);
        Task<RTCSessionDescriptionInit> createAnswer(RTCAnswerOptions options = null);
        Task setLocalDescription(RTCSessionDescriptionInit description = null);
        RTCSessionDescription localDescription { get; }
        RTCSessionDescription currentLocalDescription { get; }
        RTCSessionDescription pendingLocalDescription { get; }
        Task setRemoteDescription(RTCSessionDescriptionInit description = null);
        RTCSessionDescription remoteDescription { get; }
        RTCSessionDescription currentRemoteDescription { get; }
        RTCSessionDescription pendingRemoteDescription { get; }
        Task addIceCandidate(RTCIceCandidateInit candidate = null);
        RTCSignalingState signalingState { get; }
        RTCIceGatheringState iceGatheringState { get; }
        RTCIceConnectionState iceConnectionState { get; }
        RTCPeerConnectionState connectionState { get; }
        bool canTrickleIceCandidates { get; }
        void restartIce();
        RTCConfiguration getConfiguration();
        void setConfiguration(RTCConfiguration configuration = null);
        void close();
        event Action onnegotiationneeded;
        event Action<RTCIceCandidate> onicecandidate;
        event Action onicecandidateerror;
        event Action onsignalingstatechange;
        event Action<RTCIceConnectionState> oniceconnectionstatechange;
        event Action<RTCIceGatheringState> onicegatheringstatechange;
        event Action<RTCPeerConnectionState> onconnectionstatechange;
    }
}
